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
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace IronSmarkets.Exceptions
{
    [Serializable]
    public class OrderInvalidException : Exception
    {
        private static readonly IDictionary<Proto.Seto.OrderInvalidReason, string> Messages =
            new Dictionary<Proto.Seto.OrderInvalidReason, string> {
            { Proto.Seto.OrderInvalidReason.ORDERINVALIDINVALIDPRICE, "Invalid price" },
            { Proto.Seto.OrderInvalidReason.ORDERINVALIDINVALIDQUANTITY, "Invalid quantity" }
        };

        private readonly string _errorMessage;

        public string ErrorMessage { get { return _errorMessage; } }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected OrderInvalidException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        {
            _errorMessage = info.GetString("ErrorMessage");
        }

        internal OrderInvalidException(string errorMessage)
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

        internal static OrderInvalidException FromSeto(Proto.Seto.OrderInvalid seto)
        {
            var msg = String.Join(", ", seto.Reasons.Select(x => Messages[x]));
            return new OrderInvalidException(msg);
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
    }
}
