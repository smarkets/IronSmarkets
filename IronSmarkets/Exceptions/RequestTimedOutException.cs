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
    public class RequestTimedOutException : Exception
    {
        private const string DefaultMessage =
            "The request timed out.";

        private readonly int _timeout;

        public override string Message { get { return DefaultMessage; } }
        public int Timeout { get { return _timeout; } }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected RequestTimedOutException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        {
            _timeout = info.GetInt32("Timeout");
        }

        public RequestTimedOutException(int timeout)
        {
            _timeout = timeout;
        }

        public override IDictionary Data {
            get {
                return new Dictionary<string, object> {
                    { "Timeout", Timeout }
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

            info.AddValue("Timeout", Timeout);
            base.GetObjectData(info, context);
        }
    }
}
