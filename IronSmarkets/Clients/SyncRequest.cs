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
        ISyncRequest<TPayload>, IResponse<TResponse>, IDisposable
    {
        private readonly ulong _sequence;

        private ManualResetEvent _replied = new ManualResetEvent(false);
        private TPayload _response;
        private TResponse _data;
        private Exception _responseException;
        private int _disposed;

        protected readonly TState _state;

        public ulong Sequence { get { return _sequence; } }

        public TResponse Data
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(
                        "SyncRequest",
                        "Accessed Data property on disposed object");

                WaitOne();
                if (_responseException != null)
                    throw _responseException;
                return _data;
            }
        }

        public bool IsDisposed
        {
            get
            {
                return Thread.VolatileRead(ref _disposed) == 1;
            }
        }

        public SyncRequest(ulong sequence, TState state)
        {
            _sequence = sequence;
            _state = state;
        }

        ~SyncRequest()
        {
            Dispose(false);
        }

        public bool WaitOne()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SyncRequest",
                    "Called WaitOne on disposed object");

            return _replied.WaitOne();
        }

        public bool WaitOne(int millisecondsTimeout)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SyncRequest",
                    "Called WaitOne on disposed object");

            return _replied.WaitOne(millisecondsTimeout);
        }

        public bool WaitOne(TimeSpan timeout)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SyncRequest",
                    "Called WaitOne on disposed object");

            return _replied.WaitOne(timeout);
        }

        public bool WaitOne(int millisecondsTimeout, bool exitContext)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SyncRequest",
                    "Called WaitOne on disposed object");

            return _replied.WaitOne(millisecondsTimeout, exitContext);
        }

        public bool WaitOne(TimeSpan timeout, bool exitContext)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SyncRequest",
                    "Called WaitOne on disposed object");

            return _replied.WaitOne(timeout, exitContext);
        }

        public void SetResponse(ISmarketsClient client, TPayload response)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SyncRequest",
                    "Called SetResponse on disposed object");

            _data = Map(client, response);
            _replied.Set();
        }

        public void SetException(Exception exception)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SyncRequest",
                    "Called SetException on disposed object");

            _responseException = exception;
            _replied.Set();
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                if (disposing)
                {
                    _replied.Dispose();
                }

                _replied = null;
            }
        }

        protected abstract TResponse Map(ISmarketsClient client, TPayload payload);
    }
}
