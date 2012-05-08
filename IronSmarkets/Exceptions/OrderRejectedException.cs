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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace IronSmarkets.Exceptions
{
    [Serializable]
    public class OrderRejectedException : Exception
    {
        private static readonly IDictionary<Proto.Seto.OrderRejectedReason, string> Messages =
            new Dictionary<Proto.Seto.OrderRejectedReason, string> {
            { Proto.Seto.OrderRejectedReason.ORDERREJECTEDINSUFFICIENTFUNDS, "Insufficient funds" },
            { Proto.Seto.OrderRejectedReason.ORDERREJECTEDLIMITEXCEEDED, "Limit exceeded" },
            { Proto.Seto.OrderRejectedReason.ORDERREJECTEDMARKETNOTOPEN, "Market not open" },
            { Proto.Seto.OrderRejectedReason.ORDERREJECTEDMARKETSETTLED, "Market settled" },
            { Proto.Seto.OrderRejectedReason.ORDERREJECTEDMARKETHALTED, "Market halted" },
            { Proto.Seto.OrderRejectedReason.ORDERREJECTEDCROSSEDSELF, "Crossed self" },
            { Proto.Seto.OrderRejectedReason.ORDERREJECTEDMARKETNOTFOUND, "Market not found" },
            { Proto.Seto.OrderRejectedReason.ORDERREJECTEDSERVICETEMPORARILYUNAVAILABLE, "Service temporarily unavailable" }
        };

        private readonly string _errorMessage;

        public string ErrorMessage { get { return _errorMessage; } }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected OrderRejectedException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        {
            _errorMessage = info.GetString("ErrorMessage");
        }

        internal OrderRejectedException(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public override string Message {
            get {
                return _errorMessage;
            }
        }

        public override IDictionary Data {
            get {
                return new Dictionary<string, object> {
                    { "ErrorMessage", ErrorMessage }
                };
            }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("ErrorMessage", ErrorMessage);
            base.GetObjectData(info, context);
        }

        internal static OrderRejectedException FromSeto(Proto.Seto.OrderRejected seto)
        {
            return new OrderRejectedException(Messages[seto.Reason]);
        }
    }
}
