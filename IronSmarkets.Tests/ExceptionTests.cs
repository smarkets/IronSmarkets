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
using System.Runtime.Serialization.Formatters.Binary;

using IronSmarkets.Exceptions;
using IronSmarkets.Proto.Eto;

using Xunit;

namespace IronSmarkets.Tests
{
    public class ExceptionTests
    {
        public T SerializeAndCompare<T>(T exception) where T : class
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, exception);
                stream.Seek(0, SeekOrigin.Begin);
                var comparison = formatter.Deserialize(stream);
                var deserialized = comparison as T;
                Assert.NotNull(deserialized);
                return deserialized;
            }
        }

        [Fact]
        public void SerializableRequestTimedOutException()
        {
            var requestTimedOut = new RequestTimedOutException(60);
            Assert.DoesNotThrow(() => {
                    var requestTimedOut2 = SerializeAndCompare(requestTimedOut);
                    Assert.Equal(requestTimedOut.Timeout, requestTimedOut2.Timeout);
            });
        }

        [Fact]
        public void SerializableConnectionException()
        {
            var connectionException = new ConnectionException("foo");
            Assert.DoesNotThrow(() => {
                    var connectionException2 = SerializeAndCompare(connectionException);
                    Assert.Equal(connectionException.ErrorMessage, connectionException2.ErrorMessage);
            });
        }

        [Fact]
        public void SerializableInvalidEventQueryException()
        {
            var invalidEventQueryException = new InvalidEventQueryException("foo");
            Assert.DoesNotThrow(() => {
                    var invalidEventQueryException2 = SerializeAndCompare(invalidEventQueryException);
                    Assert.Equal(invalidEventQueryException.ErrorMessage, invalidEventQueryException2.ErrorMessage);
            });
        }

        [Fact]
        public void SerializableInvalidRequestException()
        {
            var invalidRequestException = new InvalidRequestException("foo");
            Assert.DoesNotThrow(() => {
                    var invalidRequestException2 = SerializeAndCompare(invalidRequestException);
                    Assert.Equal(invalidRequestException.ErrorMessage, invalidRequestException2.ErrorMessage);
            });
        }

        [Fact]
        public void SerializableLoginFailedException()
        {
            var loginFailedException = new LoginFailedException("foo", LogoutReason.LOGOUTUNAUTHORISED);
            Assert.DoesNotThrow(() => {
                    var loginFailedException2 = SerializeAndCompare(loginFailedException);
                    Assert.Equal(loginFailedException.ErrorMessage, loginFailedException2.ErrorMessage);
                    Assert.Equal(loginFailedException.LogoutReason, loginFailedException2.LogoutReason);
            });
        }

        [Fact]
        public void SerializableMessageStreamException()
        {
            var messageStreamException = new MessageStreamException("foo");
            Assert.DoesNotThrow(() => {
                    var messageStreamException2 = SerializeAndCompare(messageStreamException);
                    Assert.Equal(messageStreamException.ErrorMessage, messageStreamException2.ErrorMessage);
            });
        }

        [Fact]
        public void SerializableNoHandlerException()
        {
            var noHandlerException = new NoHandlerException();
            Assert.DoesNotThrow(() => {
                    SerializeAndCompare(noHandlerException);
            });
        }
    }
}
