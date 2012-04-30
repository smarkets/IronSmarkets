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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using ProtoBuf;

using IronSmarkets.Exceptions;
using IronSmarkets.Proto.Seto;
using IronSmarkets.Sockets;

namespace IronSmarkets.Tests.Mocks
{
    public interface IPayloadWrapper
    {
        Payload Payload { get; }
        bool Outgoing { get; }

        bool Expects(Payload payload);
    }

    internal sealed class ExpectsPayload : IPayloadWrapper
    {
        private readonly Payload _payload;
        private readonly byte[] _serialized;

        public Payload Payload { get { return _payload; } }
        public bool Outgoing { get { return false; } }

        public ExpectsPayload(Payload payload)
        {
            _payload = payload;
            _serialized = SerializedPayload(payload);
        }

        public bool Expects(Payload payload)
        {
            // Note: this is a very strict check as the payload *when
            // serialized* must be equivalent to the expected payload
            // when serialized
            return _serialized.SequenceEqual(SerializedPayload(payload));
        }

        private static byte[] SerializedPayload(Payload payload)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, payload);
            return stream.ToArray();
        }
    }

    internal sealed class ExpectsAny : IPayloadWrapper
    {
        public Payload Payload { get { throw new InvalidOperationException(); } }
        public bool Outgoing { get { return false; } }

        public bool Expects(Payload payload)
        {
            return true;
        }
    }

    internal sealed class OutgoingPayload : IPayloadWrapper
    {
        private readonly Payload _payload;

        public Payload Payload { get { return _payload; } }
        public bool Outgoing { get { return true; } }

        public OutgoingPayload(Payload payload)
        {
            _payload = payload;
        }

        public bool Expects(Payload payload)
        {
            throw new InvalidOperationException();
        }
    }

    public sealed class MockSessionSocket : IDisposable, ISocket<Payload>
    {
        private readonly Queue<IPayloadWrapper> _payloads =
            new Queue<IPayloadWrapper>();
        private readonly ConcurrentQueue<Payload> _incoming =
            new ConcurrentQueue<Payload>();

        // We only expect 1 waiting thread, so this WaitHandle should
        // suffice
        private readonly AutoResetEvent _incomingEvent =
            new AutoResetEvent(false);

        private bool _disposed;
        private bool _connected;

        public bool IsConnected
        {
            get
            {
                return _connected;
            }
        }

        ~MockSessionSocket()
        {
            Dispose(false);
        }

        public void Expect(Payload payload)
        {
            _payloads.Enqueue(new ExpectsPayload(payload));
        }

        public void ExpectAny()
        {
            _payloads.Enqueue(new ExpectsAny());
        }

        public void Next(Payload payload)
        {
            _payloads.Enqueue(new OutgoingPayload(payload));
        }

        public void Connect()
        {
            if (_disposed)
                throw new ObjectDisposedException(
                    "SessionSocket",
                    "Called Connect on disposed object");

            if (IsConnected)
                throw new ConnectionException(
                    "Socket already connected");

            _connected = true;
        }

        public void Disconnect()
        {
            if (_disposed)
                throw new ObjectDisposedException(
                    "SessionSocket",
                    "Called Disconnect on disposed object");

            if (IsConnected)
            {
                _connected = false;
            }
        }

        public void Write(Payload payload)
        {
            if (_disposed)
                throw new ObjectDisposedException(
                    "SessionSocket",
                    "Called Write on disposed object");

            if (!IsConnected)
                throw new ConnectionException(
                    "Socket not connected");

            _incoming.Enqueue(payload);
            _incomingEvent.Set();
        }

        public void Flush()
        {
            if (_disposed)
                throw new ObjectDisposedException(
                    "SessionSocket",
                    "Called Flush on disposed object");

            if (!IsConnected)
                throw new ConnectionException(
                    "Socket not connected");

            // Intentionally do nothing
        }

        public Payload Read()
        {
            if (_disposed)
                throw new ObjectDisposedException(
                    "SessionSocket",
                    "Called Read on disposed object");

            if (!IsConnected)
                ThrowNotConnected();

            while (true)
            {
                if (_payloads.Count == 0)
                    ThrowNotConnected();
                var next = _payloads.Peek();
                if (next.Outgoing)
                    return _payloads.Dequeue().Payload;
                // We are very impatient in unit tests
                if (!_incomingEvent.WaitOne(1000)) ThrowNotConnected();
                Payload lastIn;
                if (!_incoming.TryPeek(out lastIn)) ThrowNotConnected();
                if (!next.Expects(lastIn))
                    throw new InvalidOperationException();
                if (!_incoming.TryDequeue(out lastIn))
                    ThrowNotConnected();
                _payloads.Dequeue();
            }
        }

        private static void ThrowNotConnected()
        {
            throw new ConnectionException("Socket not connected");
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            try
            {
                if (disposing)
                {
                    Disconnect();
                }
            }
            finally
            {
                _disposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
