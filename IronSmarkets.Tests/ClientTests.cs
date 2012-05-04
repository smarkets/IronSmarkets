// Copyright (c) 2011 Smarkets Limited
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
// BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using IronSmarkets.Clients;
using IronSmarkets.Data;
using IronSmarkets.Exceptions;
using IronSmarkets.Messages;
using IronSmarkets.Sessions;
using IronSmarkets.Sockets;
using IronSmarkets.Tests.Mocks;

using Seto = IronSmarkets.Proto.Seto;
using Xunit;

namespace IronSmarkets.Tests
{
    public class ClientTests
    {
        private static readonly SocketSettings SocketSettings = new SocketSettings("mock", 3701);
        private static readonly SessionSettings SessionSettings = new SessionSettings("mockuser", "mockpassword");

        private static readonly TimeSpan DataWait = new TimeSpan(0, 0, 2);  // wait 2 seconds

        [Fact]
        public void HandleQuotes()
        {
            var socket = new MockSessionSocket();
            var mockMarketUid = new Uid(317002);
            var mockContractUid = new Uid(608008);
            var mockOrder = new NewOrder {
                Type = OrderCreateType.Limit,
                Market = mockMarketUid,
                Contract = mockContractUid,
                Side = Side.Buy,
                Quantity = new Quantity(QuantityType.PayoffCurrency, 60000),
                Price = new Price(PriceType.PercentOdds, 5714)
            };
            var builder = new EventQueryBuilder();
            builder.SetCategory("sport");
            builder.SetSport("football");
            builder.SetDateTime(new DateTime(2012,2,21));

            socket.Expect(Payloads.Sequenced(Payloads.Login("mockuser", "mockpassword"), 1));
            socket.Next(Payloads.Sequenced(Payloads.LoginResponse("00000000-0000-0000-0000-000000658a8", 2), 1));
            socket.Expect(Payloads.Sequenced(Payloads.EventsRequest(builder.GetResult()), 2));
            socket.Next(Payloads.Sequenced(Payloads.HttpFound("http://mock/api/events/sport/football/20120221/2.pb", 2), 2));
            socket.Expect(Payloads.Sequenced(Payloads.MarketSubscribe(new Uid(317002)), 3));
            socket.Next(Payloads.Sequenced(Payloads.MarketQuotes(new Uid(317002)), 3));
            socket.Expect(Payloads.Sequenced(Payloads.OrderCreate(mockOrder), 4));
            socket.Next(
                Payloads.Sequenced(
                    Payloads.ContractQuotes(
                        new Uid(608008),
                        new List<Seto.Quote> { new Seto.Quote { Price = 5714, Quantity = 60000 } },
                        Enumerable.Empty<Seto.Quote>()), 4));
            socket.Next(Payloads.Sequenced(Payloads.OrderAccepted(new Uid(82892989397900053), 4), 5));
            socket.Expect(Payloads.Sequenced(Payloads.Logout(), 5));
            socket.Next(Payloads.Sequenced(Payloads.LogoutConfirmation(), 6));

            var session = new SeqSession(socket, SessionSettings);
            IClientSettings mockSettings = new ClientSettings(SocketSettings, SessionSettings);

            var mockHttpHandler = new MockHttpFoundHandler<Seto.Events>();
            mockHttpHandler.AddDocument(MockUrls.Football20120221);
            using (var client = SmarketsClient.Create(mockSettings, session, mockHttpHandler))
            {
                mockHttpHandler.SetClient(client);
                client.Login();
                var mockEventUid = new Uid(247001);
                var mockMapResponse = client.GetEvents(builder.GetResult());
                Assert.True(mockMapResponse.WaitOne(DataWait));
                var mockMap = mockMapResponse.Data;
                Assert.True(mockMap.ContainsKey(mockEventUid));
                var mockMarket = client.MarketMap[mockMarketUid];
                var mockContract = client.ContractMap[mockContractUid];
                mockMarket.SubscribeQuotes(client);
                var marketUpdatedEvent = new ManualResetEvent(false);
                var contractUpdatedEvent = new ManualResetEvent(false);
                MarketQuotes mockQuotes = null;
                mockMarket.MarketQuotesUpdated += (sender, args) => {
                    mockQuotes = args.Quotes;
                    marketUpdatedEvent.Set();
                };
                mockContract.ContractQuotesUpdated += (sender, args) => contractUpdatedEvent.Set();
                var createResponse = client.CreateOrder(mockOrder);
                Assert.True(createResponse.WaitOne(DataWait));
                Assert.NotNull(createResponse.Data);
                Assert.True(marketUpdatedEvent.WaitOne(1000));
                Assert.Equal(mockQuotes.QuantityType, QuantityType.PayoffCurrency);
                Assert.Equal(mockQuotes.PriceType, PriceType.PercentOdds);
                Assert.Equal(mockQuotes.Uid, mockMarketUid);
                Assert.True(contractUpdatedEvent.WaitOne(1000));
                Assert.True(mockQuotes.ContractQuotes.ContainsKey(mockContractUid));
                var mockContractQuotes = mockQuotes.ContractQuotes[mockContractUid];
                Assert.Equal(mockContractQuotes.Bids.Count(), 1);
                var mockBid = mockContractQuotes.Bids.First();
                Assert.Equal<uint>(mockBid.Price.Raw, 5714);
                Assert.Equal<uint>(mockBid.Quantity.Raw, 60000);
                Assert.Equal(mockContractQuotes.Offers.Count(), 0);
                Assert.Equal(mockContractQuotes.Executions.Count(), 0);
                Assert.Equal(mockContractQuotes.QuantityType, QuantityType.PayoffCurrency);
                Assert.Equal(mockContractQuotes.PriceType, PriceType.PercentOdds);
                Assert.Equal(mockContractQuotes.Uid, mockContractUid);
                client.Logout();
            }
        }

        [Fact]
        public void MultipleOrdersAcceptedAsynchronously()
        {
            var socket = new MockSessionSocket();
            var mockMarketUid = new Uid(317002);
            var mockContractUid = new Uid(608008);
            var mockOrder = new NewOrder {
                Type = OrderCreateType.Limit,
                Market = mockMarketUid,
                Contract = mockContractUid,
                Side = Side.Buy,
                Quantity = new Quantity(QuantityType.PayoffCurrency, 60000),
                Price = new Price(PriceType.PercentOdds, 5714)
            };

            socket.Expect(Payloads.Sequenced(Payloads.Login("mockuser", "mockpassword"), 1));
            socket.Next(Payloads.Sequenced(Payloads.LoginResponse("00000000-0000-0000-0000-000000658a8", 2), 1));
            socket.Expect(Payloads.Sequenced(Payloads.OrderCreate(mockOrder), 2));
            socket.Expect(Payloads.Sequenced(Payloads.OrderCreate(mockOrder), 3));
            socket.Next(Payloads.Sequenced(Payloads.OrderAccepted(new Uid(82892989397900053), 3), 2));
            socket.Next(Payloads.Sequenced(Payloads.OrderAccepted(new Uid(82892989397900054), 2), 3));
            socket.Expect(Payloads.Sequenced(Payloads.Logout(), 4));
            socket.Next(Payloads.Sequenced(Payloads.LogoutConfirmation(), 4));

            var session = new SeqSession(socket, SessionSettings);
            IClientSettings mockSettings = new ClientSettings(SocketSettings, SessionSettings);

            using (var client = SmarketsClient.Create(mockSettings, session))
            {
                client.Login();
                var mockOrderResponse1 = client.CreateOrder(mockOrder);
                var mockOrderResponse2 = client.CreateOrder(mockOrder);
                Assert.True(mockOrderResponse1.WaitOne(DataWait));
                Assert.True(mockOrderResponse2.WaitOne(DataWait));
                Assert.NotNull(mockOrderResponse1.Data);
                Assert.NotNull(mockOrderResponse2.Data);
                client.Logout();
            }
        }

        [Fact]
        public void MultipleOrderCancelsAsynchronously()
        {
            var socket = new MockSessionSocket();
            var mockMarketUid = new Uid(317002);
            var mockContractUid = new Uid(608008);
            var mockOrder = new NewOrder {
                Type = OrderCreateType.Limit,
                Market = mockMarketUid,
                Contract = mockContractUid,
                Side = Side.Buy,
                Quantity = new Quantity(QuantityType.PayoffCurrency, 60000),
                Price = new Price(PriceType.PercentOdds, 5714)
            };

            socket.Expect(Payloads.Sequenced(Payloads.Login("mockuser", "mockpassword"), 1));
            socket.Next(Payloads.Sequenced(Payloads.LoginResponse("00000000-0000-0000-0000-000000658a8", 2), 1));
            socket.Expect(Payloads.Sequenced(Payloads.OrderCreate(mockOrder), 2));
            socket.Expect(Payloads.Sequenced(Payloads.OrderCreate(mockOrder), 3));
            socket.Next(Payloads.Sequenced(Payloads.OrderAccepted(new Uid(82892989397900053), 3), 2));
            socket.Next(Payloads.Sequenced(Payloads.OrderAccepted(new Uid(82892989397900054), 2), 3));
            socket.Expect(Payloads.Sequenced(Payloads.OrderCancel(new Uid(82892989397900053)), 4));
            socket.Expect(Payloads.Sequenced(Payloads.OrderCancel(new Uid(82892989397900054)), 5));
            socket.Next(Payloads.Sequenced(Payloads.OrderCancelled(new Uid(82892989397900053)), 4));
            socket.Next(Payloads.Sequenced(Payloads.OrderCancelled(new Uid(82892989397900054)), 5));
            socket.Expect(Payloads.Sequenced(Payloads.Logout(), 6));
            socket.Next(Payloads.Sequenced(Payloads.LogoutConfirmation(), 6));

            var session = new SeqSession(socket, SessionSettings);
            IClientSettings mockSettings = new ClientSettings(SocketSettings, SessionSettings);

            using (var client = SmarketsClient.Create(mockSettings, session))
            {
                client.Login();
                var mockOrderResponse1 = client.CreateOrder(mockOrder);
                var mockOrderResponse2 = client.CreateOrder(mockOrder);
                Assert.True(mockOrderResponse1.WaitOne(DataWait));
                Assert.True(mockOrderResponse2.WaitOne(DataWait));
                Assert.NotNull(mockOrderResponse1.Data);
                Assert.NotNull(mockOrderResponse2.Data);
                var mockOrderCancelResponse2 = client.CancelOrder(mockOrderResponse2.Data);
                var mockOrderCancelResponse1 = client.CancelOrder(mockOrderResponse1.Data);
                Assert.True(mockOrderCancelResponse1.WaitOne(DataWait));
                Assert.True(mockOrderCancelResponse2.WaitOne(DataWait));
                Assert.Equal(mockOrderCancelResponse1.Data, OrderCancelledReason.MemberRequested);
                Assert.Equal(mockOrderCancelResponse2.Data, OrderCancelledReason.MemberRequested);
                client.Logout();
            }
        }

        [Fact]
        public void ReceiverDeadlockTest()
        {
            var socket = new MockSessionSocket();
            socket.Expect(Payloads.Sequenced(Payloads.Login("mockuser", "mockpassword"), 1));
            socket.Next(Payloads.Sequenced(Payloads.LoginResponse("00000000-0000-0000-0000-000000658a8", 2), 1));
            socket.Expect(Payloads.Sequenced(Payloads.Ping(), 2));
            socket.Next(Payloads.Sequenced(Payloads.Pong(), 2));
            socket.Expect(Payloads.Sequenced(Payloads.Ping(), 3));

            var session = new SeqSession(socket, SessionSettings);
            IClientSettings mockSettings = new ClientSettings(SocketSettings, SessionSettings);
            ManualResetEvent waiter = new ManualResetEvent(false);
            using (var client = SmarketsClient.Create(mockSettings, session))
            {
                client.Login();
                client.AddPayloadHandler(
                    (payload) => {
                        Assert.Throws<ReceiverDeadlockException>(() => client.Ping());
                        waiter.Set();
                        return true;
                    });
                client.Ping();
                waiter.WaitOne();
            }
        }
    }
}
