// Copyright (c) 2011-2012 Smarkets Limited
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
using System.Threading;

namespace IronSmarkets.Clients
{
    internal abstract class SyncRequest<TPayload, TResponse, TState> :
        ISyncRequest<TPayload>, IResponse<TResponse>
    {
        private readonly ManualResetEvent _replied =
            new ManualResetEvent(false);
        private readonly ulong _sequence;

        private TPayload _response;
        private TResponse _data;
        private Exception _responseException;

        protected readonly TState _state;

        public ulong Sequence { get { return _sequence; } }

        public TResponse Data
        {
            get
            {
                _replied.WaitOne();
                _replied.Close();
                if (_responseException != null)
                    throw _responseException;
                return _data;
            }
        }

        public SyncRequest(ulong sequence, TState state)
        {
            _sequence = sequence;
            _state = state;
        }

        public void SetResponse(ISmarketsClient client, TPayload response)
        {
            _data = Map(client, response);
            _replied.Set();
        }

        public void SetException(Exception exception)
        {
            _responseException = exception;
            _replied.Set();
        }

        protected abstract TResponse Map(ISmarketsClient client, TPayload payload);
    }
}
