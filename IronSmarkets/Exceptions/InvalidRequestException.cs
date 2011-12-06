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

namespace IronSmarkets.Exceptions
{
    public class InvalidRequestException : Exception
    {
        private static readonly IDictionary<Proto.Seto.InvalidRequestType, string> Messages =
            new Dictionary<Proto.Seto.InvalidRequestType, string> {
            { Proto.Seto.InvalidRequestType.INVALIDREQUESTDATEOUTOFRANGE, "Date out of range" }
        };

        private readonly string _errorMessage;

        public string ErrorMessage { get { return _errorMessage; } }

        private InvalidRequestException(string errorMessage)
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

        internal static InvalidRequestException FromSeto(Proto.Seto.InvalidRequest seto)
        {
            return new InvalidRequestException(Messages[seto.Type]);
        }
    }
}
