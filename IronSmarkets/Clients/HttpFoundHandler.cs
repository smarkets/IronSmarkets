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
using System.Net;
using System.IO;
using System.Threading;

using log4net;
using ProtoBuf;

using IronSmarkets.Exceptions;

namespace IronSmarkets.Clients
{
    internal sealed class HttpFoundHandler<T>
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int _timeout;

        public HttpFoundHandler(int timeout)
        {
            _timeout = timeout;
        }

        public void BeginFetchHttpFound(
            SyncRequest<T> syncRequest, Proto.Seto.Payload payload)
        {
            var url = payload.HttpFound.Url;
            if (Log.IsDebugEnabled) Log.Debug(
                string.Format("Fetching payload from URL {0}", url));
            var req = (HttpWebRequest)WebRequest.Create(url);
            var state = new Tuple<HttpWebRequest, SyncRequest<T>>(
                req, syncRequest);
            var result = req.BeginGetResponse(
                FetchHttpFoundCallback, state);
            ThreadPool.RegisterWaitForSingleObject(
                result.AsyncWaitHandle,
                HttpFoundTimeoutCallback,
                state, _timeout, true);
        }

        private void FetchHttpFoundCallback(IAsyncResult result)
        {
            var stateTuple = (Tuple<HttpWebRequest, SyncRequest<T>>)result.AsyncState;
            if (Log.IsDebugEnabled) Log.Debug(
                string.Format("Web request callback for URL {0}", stateTuple.Item1.RequestUri));
            try
            {
                using (var resp = stateTuple.Item1.EndGetResponse(result))
                {
                    if (Log.IsDebugEnabled) Log.Debug("Received a response, deserializing");
                    Stream receiveStream = resp.GetResponseStream();
                    stateTuple.Item2.Response = Serializer.Deserialize<T>(receiveStream);
                }
            }
            catch (WebException wex)
            {
                if (wex.Message == "Aborted.")
                {
                    stateTuple.Item2.SetException(
                        new RequestTimedOutException(_timeout));
                }
                else
                {
                    stateTuple.Item2.SetException(wex);
                }
            }
            catch (Exception ex)
            {
                stateTuple.Item2.SetException(ex);
            }
        }

        private void HttpFoundTimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                var stateTuple = (Tuple<HttpWebRequest, SyncRequest<T>>)state;
                stateTuple.Item1.Abort();
            }
        }
    }
}
