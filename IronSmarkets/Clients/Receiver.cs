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

using System.IO;
using System.Threading;

using log4net;

using IronSmarkets.Exceptions;
using IronSmarkets.Sessions;

namespace IronSmarkets.Clients
{
    internal sealed class Receiver<T> where T : Proto.Seto.IPayload
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Thread _loop;
        private readonly ISession<T> _session;

        private volatile bool _complete;

        public Receiver(ISession<T> session)
        {
            _session = session;
            _loop = new Thread(Loop) {
                Name = "receiver"
            };
        }

        public void Start()
        {
            _complete = false;
            _loop.Start();
        }

        public void Stop()
        {
            _complete = true;
            _loop.Join();
        }

        private void Loop()
        {
            while (!_complete)
            {
                try
                {
                    var payload = _session.Receive();
                    if (payload.IsLogoutConfirmation())
                    {
                        Log.Info(
                            "Received a logout confirmation; " +
                            "assuming socket will close");
                        _complete = true;
                    }
                }
                catch (IOException ex)
                {
                    Log.Warn("Receiving socket timed out", ex);
                    _complete = true;
                }
                catch (ConnectionException cex)
                {
                    Log.Warn("Caught connection exceptions", cex);
                    _complete = true;
                }
            }
        }
    }
}
