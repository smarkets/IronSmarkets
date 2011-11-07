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
    internal sealed class RequestState<T> : Tuple<HttpWebRequest, SyncRequest<T>>
    {
        public HttpWebRequest WebRequest { get { return Item1; } }
        public SyncRequest<T> Request { get { return Item2; } }

        public RequestState(HttpWebRequest webRequest, SyncRequest<T> request) : base(webRequest, request)
        {
        }
    }

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
            var state = new RequestState<T>(req, syncRequest);
            var result = req.BeginGetResponse(
                FetchHttpFoundCallback, state);
            ThreadPool.RegisterWaitForSingleObject(
                result.AsyncWaitHandle,
                HttpFoundTimeoutCallback,
                state, _timeout, true);
        }

        private void FetchHttpFoundCallback(IAsyncResult result)
        {
            var requestState = result.AsyncState as RequestState<T>;
            if (requestState == null) throw new ArgumentNullException("result");
            if (Log.IsDebugEnabled) Log.Debug(
                string.Format("Web request callback for URL {0}", requestState.WebRequest.RequestUri));
            try
            {
                using (var resp = requestState.WebRequest.EndGetResponse(result))
                {
                    if (Log.IsDebugEnabled) Log.Debug("Received a response, deserializing");
                    Stream receiveStream = resp.GetResponseStream();
                    requestState.Request.Response = Serializer.Deserialize<T>(receiveStream);
                }
            }
            catch (WebException wex)
            {
                if (wex.Message == "Aborted.")
                {
                    requestState.Request.SetException(
                        new RequestTimedOutException(_timeout));
                }
                else
                {
                    requestState.Request.SetException(wex);
                }
            }
            catch (Exception ex)
            {
                requestState.Request.SetException(ex);
            }
        }

        private void HttpFoundTimeoutCallback(object state, bool timedOut)
        {
            if (!timedOut) return;
            var requestState = state as RequestState<T>;
            if (requestState != null) requestState.WebRequest.Abort();
        }
    }
}