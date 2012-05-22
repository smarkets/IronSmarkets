// Copyright (c) 2012 Smarkets Limited
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

using IronSmarkets.Data;
using IronSmarkets.Proto.Seto;
#if NET35
using IronSmarkets.System;
#endif

using Eto = IronSmarkets.Proto.Eto;

namespace IronSmarkets.Messages
{
    public static class Payloads
    {
        public static Payload Sequenced(Payload payload, ulong sequence)
        {
            if (payload.EtoPayload == null)
                payload.EtoPayload = new Eto.Payload();
            payload.EtoPayload.Seq = sequence;
            return payload;
        }

        public static Payload Login(string username, string password)
        {
            return new Payload {
                Type = PayloadType.PAYLOADLOGIN,
                EtoPayload = new Eto.Payload {
                    Type = Eto.PayloadType.PAYLOADLOGIN
                },
                Login = new Login {
                    Username = username,
                    Password = password
                }
            };
        }

        public static Payload LoginResponse(string session, ulong reset)
        {
            return new Payload {
                Type = PayloadType.PAYLOADETO,
                EtoPayload = new Eto.Payload {
                    Type = Eto.PayloadType.PAYLOADLOGINRESPONSE,
                    LoginResponse = new Eto.LoginResponse {
                        Session = session,
                        Reset = reset
                    }
                }
            };
        }

        public static Payload Logout()
        {
            return LogoutReason(Eto.LogoutReason.LOGOUTNONE);
        }

        public static Payload LogoutConfirmation()
        {
            return LogoutReason(Eto.LogoutReason.LOGOUTCONFIRMATION);
        }

        private static Payload LogoutReason(Eto.LogoutReason reason)
        {
            return new Payload {
                Type = PayloadType.PAYLOADETO,
                EtoPayload = new Eto.Payload {
                    Type = Eto.PayloadType.PAYLOADLOGOUT,
                    Logout = new Eto.Logout {
                        Reason = reason
                    }
                }
            };
        }

        public static Payload Heartbeat()
        {
            return new Payload {
                Type = PayloadType.PAYLOADETO,
                EtoPayload = new Eto.Payload {
                    Type = Eto.PayloadType.PAYLOADHEARTBEAT
                }
            };
        }

        public static Payload Replay(ulong sequence)
        {
            return new Payload {
                Type = PayloadType.PAYLOADETO,
                EtoPayload = new Eto.Payload {
                    Type = Eto.PayloadType.PAYLOADREPLAY,
                    Replay = new Eto.Replay {
                        Seq = sequence
                    }
                }
            };
        }

        public static Payload Ping()
        {
            return new Payload {
                Type = PayloadType.PAYLOADETO,
                EtoPayload = new Eto.Payload {
                    Type = Eto.PayloadType.PAYLOADPING
                }
            };
        }

        public static Payload Pong()
        {
            return new Payload {
                Type = PayloadType.PAYLOADETO,
                EtoPayload = new Eto.Payload {
                    Type = Eto.PayloadType.PAYLOADPONG
                }
            };
        }

        public static Payload MarketSubscribe(Uid market)
        {
            return new Payload {
                Type = PayloadType.PAYLOADMARKETSUBSCRIBE,
                MarketSubscribe = new MarketSubscribe {
                    Market = market.ToUuid128()
                }
            };
        }

        public static Payload MarketQuotes(Uid market)
        {
            return new Payload {
                Type = PayloadType.PAYLOADMARKETQUOTES,
                MarketQuotes = new Proto.Seto.MarketQuotes {
                    Market = market.ToUuid128(),
                    PriceType = Proto.Seto.PriceType.PRICEPERCENTODDS,
                    QuantityType = Proto.Seto.QuantityType.QUANTITYPAYOFFCURRENCY
                }
            };
        }

        public static Payload ContractQuotes(Uid contract,
                                             IEnumerable<Proto.Seto.Quote> bids,
                                             IEnumerable<Proto.Seto.Quote> offers)
        {
            var payload = new Payload {
                Type = PayloadType.PAYLOADCONTRACTQUOTES,
                ContractQuotes = new Proto.Seto.ContractQuotes {
                    Contract = contract.ToUuid128()
                }
            };

            payload.ContractQuotes.Bids.AddRange(bids);
            payload.ContractQuotes.Offers.AddRange(offers);

            return payload;
        }

        public static Payload MarketUnsubscribe(Uid market)
        {
            return new Payload {
                Type = PayloadType.PAYLOADMARKETUNSUBSCRIBE,
                MarketUnsubscribe = new MarketUnsubscribe {
                    Market = market.ToUuid128()
                }
            };
        }

        public static Payload EventsRequest(EventQuery query)
        {
            return new Payload {
                Type = PayloadType.PAYLOADEVENTSREQUEST,
                EventsRequest = query.ToEventsRequest()
            };
        }

        public static Payload HttpFound(string url, ulong sequence)
        {
            return new Payload {
                Type = PayloadType.PAYLOADHTTPFOUND,
                HttpFound = new HttpFound {
                    Seq = sequence,
                    Url = url
                }
            };
        }

        public static Payload AccountStateRequest(Uid? account)
        {
            var request = new AccountStateRequest();
            if (account != null)
                request.Account = account.Value.ToUuid128();
            return new Payload {
                Type = PayloadType.PAYLOADACCOUNTSTATEREQUEST,
                AccountStateRequest = request
            };
        }

        public static Payload MarketQuotesRequest(Uid market)
        {
            return new Payload {
                Type = PayloadType.PAYLOADMARKETQUOTESREQUEST,
                MarketQuotesRequest = new MarketQuotesRequest {
                    Market = market.ToUuid128()
                }
            };
        }

        public static Payload OrdersForAccountRequest()
        {
            return new Payload {
                Type = PayloadType.PAYLOADORDERSFORACCOUNTREQUEST,
                OrdersForAccountRequest = new OrdersForAccountRequest()
            };
        }

        public static Payload OrdersForAccount(IEnumerable<Data.Order> orders)
        {
            var ordersForAccount = new OrdersForAccount();
            OrdersForMarket ordersForMarket = null;
            OrdersForContract ordersForContract = null;
            OrdersForPrice ordersForPrice = null;
            Data.Order prevOrder = null;

            foreach (var order in orders.OrderBy(x => new Tuple<Uid, Uid, Data.Side, Price>(x.Market, x.Contract, x.Side, x.Price)))
            {
                if (ordersForMarket == null || order.Market != prevOrder.Market)
                {
                    ordersForMarket = new OrdersForMarket {
                        Market = order.Market.ToUuid128(),
                        PriceType = order.Price.SetoType
                    };
                    ordersForAccount.Markets.Add(ordersForMarket);
                }

                if (ordersForContract == null || order.Contract != prevOrder.Contract)
                {
                    ordersForContract = new OrdersForContract { Contract = order.Contract.ToUuid128() };
                    ordersForMarket.Contracts.Add(ordersForContract);
                }

                if (ordersForPrice == null
                    || order.Price != prevOrder.Price
                    || order.Side != prevOrder.Side)
                {
                    ordersForPrice = new OrdersForPrice { Price = order.Price.Raw };
                    if (order.Side == Data.Side.Buy)
                        ordersForContract.Bids.Add(ordersForPrice);
                    if (order.Side == Data.Side.Sell)
                        ordersForContract.Offers.Add(ordersForPrice);
                }

                ordersForPrice.Orders.Add(order.State.ToSeto());
                prevOrder = order;
            }

            return new Payload {
                Type = PayloadType.PAYLOADORDERSFORACCOUNT,
                OrdersForAccount = ordersForAccount
            };
        }

        public static Payload OrdersForMarketRequest(Uid market)
        {
            return new Payload {
                Type = PayloadType.PAYLOADORDERSFORMARKETREQUEST,
                    OrdersForMarketRequest = new OrdersForMarketRequest {
                    Market = market.ToUuid128()
                }
            };
        }

        public static Payload OrderCancel(Uid order)
        {
            return new Payload {
                Type = PayloadType.PAYLOADORDERCANCEL,
                    OrderCancel = new OrderCancel {
                    Order = order.ToUuid128()
                }
            };
        }

        public static Payload OrderCreate(NewOrder order)
        {
            return new Payload {
                Type = PayloadType.PAYLOADORDERCREATE,
                OrderCreate = order.ToOrderCreate()
            };
        }

        public static Payload OrderAccepted(Uid order, ulong sequence)
        {
            return new Payload {
                Type = PayloadType.PAYLOADORDERACCEPTED,
                OrderAccepted = new OrderAccepted {
                    Seq = sequence,
                    Order = order.ToUuid128()
                }
            };
        }

        public static Payload OrderCancelled(Uid order)
        {
            return new Payload {
                Type = PayloadType.PAYLOADORDERCANCELLED,
                OrderCancelled = new OrderCancelled {
                    Order = order.ToUuid128(),
                    Reason = Proto.Seto.OrderCancelledReason.ORDERCANCELLEDMEMBERREQUESTED
                }
            };
        }
    }
}
