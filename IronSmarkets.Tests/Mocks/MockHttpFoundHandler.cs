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

using System.Collections.Generic;
using System.IO;

using IronSmarkets.Clients;

using log4net;
using ProtoBuf;

namespace IronSmarkets.Tests.Mocks
{
#if NET40
    public interface IMockHttpDocument<out T>
#else
    public interface IMockHttpDocument<T>
#endif
    {
        string Url { get; }
        T Payload { get; }
    }

    public sealed class MockHttpFoundHandler<T> : IAsyncHttpFoundHandler<T>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MockHttpFoundHandler<T>));

        private ISmarketsClient _client;

        private readonly IDictionary<string, T> _documents = new Dictionary<string, T>();

        public void SetClient(ISmarketsClient client)
        {
            _client = client;
        }

        public void AddDocument(IMockHttpDocument<T> document)
        {
            AddDocument(document.Url, document.Payload);
        }

        public void AddDocument(string url, T payload)
        {
            _documents[url] = payload;
        }

        public void BeginFetchHttpFound(
            ISyncRequest<T> syncRequest, Proto.Seto.Payload payload)
        {
            var url = payload.HttpFound.Url;
            if (_documents.ContainsKey(url))
            {
                if (Log.IsDebugEnabled) Log.Debug(
                    string.Format("Returning canned data for URL {0}", url));
                syncRequest.SetResponse(_client, _documents[url]);
            }
            else
            {
                if (Log.IsDebugEnabled) Log.Debug(
                    string.Format("Simulating a 404 for URL {0}", url));
                syncRequest.SetResponse(_client, Serializer.Deserialize<T>(new MemoryStream()));
            }
        }
    }
}
