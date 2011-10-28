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
using System.IO;
using System.Net.Security;
using System.Threading;

namespace IronSmarkets.Sockets
{
    internal sealed class StateObject
    {
        private readonly ManualResetEvent _done = new ManualResetEvent(false);

        public int BytesRead { get; set; }
        public ManualResetEvent Done { get { return _done; } }
    }

    /// <summary>
    ///   The purpose of this class is to wrap SslStream and provide
    ///   thread-safe Read and Write blocking operations. The reason
    ///   this is necessary is because NetworkStream is thread-safe
    ///   for simultaneous reads and writes as long as there is a
    ///   single reader and a single writer. SslStream, on the other
    ///   hand, does not guarantee this safety. However, we can use
    ///   BeginRead/BeginWrite to do asynchronous operations and
    ///   simply signal to the main thread that they've
    ///   completed. Performance will suffer slightly, but TLS network
    ///   performance incurs its own buffering penalties by nature.
    /// </summary>
    internal sealed class SafeStream : Stream
    {
        private readonly SslStream _stream;

        public SafeStream(SslStream stream)
        {
            _stream = stream;
        }

        public override bool CanRead { get { return _stream.CanRead; } }
        public override bool CanSeek { get { return _stream.CanSeek; } }
        public override bool CanWrite { get { return _stream.CanWrite; } }
        public override long Length { get { return _stream.Length; } }
        public override long Position {
            get
            {
                return _stream.Position;
            }
            set
            {
                _stream.Position = value;
            }
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long length)
        {
            _stream.SetLength(length);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            StateObject state = new StateObject();
            _stream.BeginRead(buffer, offset, count, ReadCallback, state);
            state.Done.WaitOne();
            return state.BytesRead;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            StateObject state = new StateObject();
            _stream.BeginWrite(buffer, offset, count, WriteCallback, state);
            state.Done.WaitOne();
        }

        private void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            state.BytesRead = _stream.EndRead(ar);
            state.Done.Set();
        }

        private void WriteCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            _stream.EndWrite(ar);
            state.Done.Set();
        }
    }
}
