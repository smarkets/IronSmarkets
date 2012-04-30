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
using System.Linq;
using System.Threading;

using IronSmarkets.Clients;
using IronSmarkets.Data;
using IronSmarkets.Sessions;
using IronSmarkets.Sockets;
using IronSmarkets.Tests.Mocks;

using Eto = IronSmarkets.Proto.Eto;
using Seto = IronSmarkets.Proto.Seto;
using Xunit;

namespace IronSmarkets.Tests
{
    public class ClientTests
    {
        private static readonly SocketSettings SocketSettings = new SocketSettings("mock", 3701);
        private static readonly SessionSettings SessionSettings = new SessionSettings("mockuser", "mockpassword");

        [Fact]
        public void HandleQuotes()
        {
            var socket = new MockSessionSocket();

            socket.Expect(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADLOGIN,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADLOGIN,
                        Seq = 1
                    },
                    Login = new Seto.Login {
                        Username = "mockuser",
                        Password = "mockpassword"
                    }
                });

            socket.Next(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADETO,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADLOGINRESPONSE,
                        Seq = 1,
                        LoginResponse = new Eto.LoginResponse {
                            Session = "00000000-0000-0000-0000-000000658a8",
                            Reset = 2
                        }
                    }
                });

            socket.Expect(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADEVENTSREQUEST,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADNONE,
                        Seq = 2
                    },
                    EventsRequest = new Seto.EventsRequest {
                        Type = Seto.EventsRequestType.EVENTSREQUESTSPORTBYDATE,
                        ContentType = Seto.ContentType.CONTENTTYPEPROTOBUF,
                        SportByDate = new Seto.SportByDate {
                            Type = Seto.SportByDateType.SPORTBYDATEFOOTBALL,
                            Date = new Seto.Date {
                                Year = 2012,
                                Month = 2,
                                Day = 21
                            }
                        }
                    }
                });

            socket.Next(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADHTTPFOUND,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADNONE,
                        Seq = 2
                    },
                    HttpFound = new Seto.HttpFound {
                        Seq = 2,
                        Url = "http://mock/api/events/sport/football/20120221/2.pb"
                    }
                });

            socket.Expect(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADMARKETSUBSCRIBE,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADNONE,
                        Seq = 3
                    },
                    MarketSubscribe = new Seto.MarketSubscribe {
                        Market = new Seto.Uuid128 {
                            Low = 317002,
                            High = 0
                        }
                    }
                });

            socket.Next(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADMARKETQUOTES,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADNONE,
                        Seq = 3
                    },
                    MarketQuotes = new Seto.MarketQuotes {
                        Market = new Seto.Uuid128 {
                            Low = 317002
                        },
                        PriceType = Seto.PriceType.PRICEPERCENTODDS,
                        QuantityType = Seto.QuantityType.QUANTITYPAYOFFCURRENCY
                    }
                });

            socket.Expect(new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADORDERCREATE,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADNONE,
                        Seq = 4
                    },
                    OrderCreate = new Seto.OrderCreate {
                        Type = Seto.OrderCreateType.ORDERCREATELIMIT,
                        Market = new Seto.Uuid128 {
                            Low = 317002,
                            High = 0
                        },
                        Contract = new Seto.Uuid128 {
                            Low = 608008,
                            High = 0
                        },
                        Side = Seto.Side.SIDEBUY,
                        QuantityType = Seto.QuantityType.QUANTITYPAYOFFCURRENCY,
                        Quantity = 60000,
                        Price = 5714
                    }
                });

            var contractQuotes = new Seto.Payload {
                Type = Seto.PayloadType.PAYLOADCONTRACTQUOTES,
                EtoPayload = new Eto.Payload {
                    Type = Eto.PayloadType.PAYLOADNONE,
                    Seq = 4
                },
                ContractQuotes = new Seto.ContractQuotes {
                    Contract = new Seto.Uuid128 {
                        Low = 608008
                    }
                }
            };

            contractQuotes.ContractQuotes.Bids.Add(
                new Seto.Quote {
                    Price = 5714,
                    Quantity = 60000
                });

            socket.Next(contractQuotes);

            socket.Next(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADORDERACCEPTED,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADNONE,
                        Seq = 5
                    },
                    OrderAccepted = new Seto.OrderAccepted {
                        Seq = 4,
                        Order = new Seto.Uuid128 {
                            Low = 82892989397900053
                        }
                    }
                });

            socket.Expect(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADETO,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADLOGOUT,
                        Seq = 5,
                        Logout = new Eto.Logout {
                            Reason = Eto.LogoutReason.LOGOUTNONE
                        }
                    }
                });

            socket.Next(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADETO,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADLOGOUT,
                        Seq = 6,
                        Logout = new Eto.Logout {
                            Reason = Eto.LogoutReason.LOGOUTCONFIRMATION
                        }
                    }
                });

            var session = new SeqSession(socket, SessionSettings);
            IClientSettings mockSettings = new ClientSettings(SocketSettings, SessionSettings);

            var mockHttpHandler = new MockHttpFoundHandler<Seto.Events>();
            mockHttpHandler.AddDocument(MockUrls.Football20120221);
            using (var client = SmarketsClient.Create(mockSettings, session, mockHttpHandler))
            {
                mockHttpHandler.SetClient(client);
                client.Login();
                var builder = new EventQueryBuilder();
                builder.SetCategory("sport");
                builder.SetSport("football");
                builder.SetDateTime(new DateTime(2012,2,21));
                var mockEventUid = new Uid(247001);
                var mockMarketUid = new Uid(317002);
                var mockContractUid = new Uid(608008);
                var mockMap = client.GetEvents(builder.GetResult()).Data;
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
                var mockOrder = new NewOrder {
                    Type = OrderCreateType.Limit,
                    Market = mockMarketUid,
                    Contract = mockContractUid,
                    Side = Side.Buy,
                    Quantity = new Quantity(QuantityType.PayoffCurrency, 60000),
                    Price = new Price(PriceType.PercentOdds, 5714)
                };
                Assert.NotNull(client.CreateOrder(mockOrder).Data);
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

            socket.Expect(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADLOGIN,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADLOGIN,
                        Seq = 1
                    },
                    Login = new Seto.Login {
                        Username = "mockuser",
                        Password = "mockpassword"
                    }
                });

            socket.Next(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADETO,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADLOGINRESPONSE,
                        Seq = 1,
                        LoginResponse = new Eto.LoginResponse {
                            Session = "00000000-0000-0000-0000-000000658a8",
                            Reset = 2
                        }
                    }
                });

            socket.Expect(new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADORDERCREATE,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADNONE,
                        Seq = 2
                    },
                    OrderCreate = new Seto.OrderCreate {
                        Type = Seto.OrderCreateType.ORDERCREATELIMIT,
                        Market = new Seto.Uuid128 {
                            Low = 317002,
                            High = 0
                        },
                        Contract = new Seto.Uuid128 {
                            Low = 608008,
                            High = 0
                        },
                        Side = Seto.Side.SIDEBUY,
                        QuantityType = Seto.QuantityType.QUANTITYPAYOFFCURRENCY,
                        Quantity = 60000,
                        Price = 5714
                    }
                });

            socket.Expect(new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADORDERCREATE,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADNONE,
                        Seq = 3
                    },
                    OrderCreate = new Seto.OrderCreate {
                        Type = Seto.OrderCreateType.ORDERCREATELIMIT,
                        Market = new Seto.Uuid128 {
                            Low = 317002,
                            High = 0
                        },
                        Contract = new Seto.Uuid128 {
                            Low = 608008,
                            High = 0
                        },
                        Side = Seto.Side.SIDEBUY,
                        QuantityType = Seto.QuantityType.QUANTITYPAYOFFCURRENCY,
                        Quantity = 60000,
                        Price = 5714
                    }
                });

            socket.Next(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADORDERACCEPTED,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADNONE,
                        Seq = 2
                    },
                    OrderAccepted = new Seto.OrderAccepted {
                        Seq = 3,
                        Order = new Seto.Uuid128 {
                            Low = 82892989397900053
                        }
                    }
                });

            socket.Next(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADORDERACCEPTED,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADNONE,
                        Seq = 3
                    },
                    OrderAccepted = new Seto.OrderAccepted {
                        Seq = 2,
                        Order = new Seto.Uuid128 {
                            Low = 82892989397900054
                        }
                    }
                });

            socket.Expect(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADETO,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADLOGOUT,
                        Seq = 4,
                        Logout = new Eto.Logout {
                            Reason = Eto.LogoutReason.LOGOUTNONE
                        }
                    }
                });

            socket.Next(
                new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADETO,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADLOGOUT,
                        Seq = 4,
                        Logout = new Eto.Logout {
                            Reason = Eto.LogoutReason.LOGOUTCONFIRMATION
                        }
                    }
                });

            var session = new SeqSession(socket, SessionSettings);
            IClientSettings mockSettings = new ClientSettings(SocketSettings, SessionSettings);

            using (var client = SmarketsClient.Create(mockSettings, session))
            {
                client.Login();
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
                var mockOrderResponse1 = client.CreateOrder(mockOrder);
                var mockOrderResponse2 = client.CreateOrder(mockOrder);
                Assert.NotNull(mockOrderResponse1.Data);
                Assert.NotNull(mockOrderResponse2.Data);
                client.Logout();
            }
        }
    }
}
