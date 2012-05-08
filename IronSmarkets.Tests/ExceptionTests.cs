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
        public void SerializableConnectionException()
        {
            var x = new ConnectionException("foo");
            Assert.DoesNotThrow(() => {
                    var y = SerializeAndCompare(x);
                    Assert.Equal(x.ErrorMessage, y.ErrorMessage);
            });
        }

        [Fact]
        public void SerializableInvalidEventQueryException()
        {
            var x = new InvalidEventQueryException("foo");
            Assert.DoesNotThrow(() => {
                    var y = SerializeAndCompare(x);
                    Assert.Equal(x.ErrorMessage, y.ErrorMessage);
            });
        }

        [Fact]
        public void SerializableInvalidRequestException()
        {
            var x = new InvalidRequestException("foo");
            Assert.DoesNotThrow(() => {
                    var y = SerializeAndCompare(x);
                    Assert.Equal(x.ErrorMessage, y.ErrorMessage);
            });
        }

        [Fact]
        public void SerializableLoginFailedException()
        {
            var x = new LoginFailedException("foo", LogoutReason.LOGOUTUNAUTHORISED);
            Assert.DoesNotThrow(() => {
                    var y = SerializeAndCompare(x);
                    Assert.Equal(x.ErrorMessage, y.ErrorMessage);
                    Assert.Equal(x.LogoutReason, y.LogoutReason);
            });
        }

        [Fact]
        public void SerializableMessageStreamException()
        {
            var x = new MessageStreamException("foo");
            Assert.DoesNotThrow(() => {
                    var y = SerializeAndCompare(x);
                    Assert.Equal(x.ErrorMessage, y.ErrorMessage);
            });
        }

        [Fact]
        public void SerializableNoHandlerException()
        {
            Assert.DoesNotThrow(() => {
                    SerializeAndCompare(new NoHandlerException());
            });
        }

        [Fact]
        public void SerializableNotLoggedInException()
        {
            Assert.DoesNotThrow(() => {
                    SerializeAndCompare(new NotLoggedInException());
            });
        }

        [Fact]
        public void SerializableOrderCancelRejectedException()
        {
            var x = new OrderCancelRejectedException("foo");
            Assert.DoesNotThrow(() => {
                    var y = SerializeAndCompare(x);
                    Assert.Equal(x.ErrorMessage, y.ErrorMessage);
            });
        }

        [Fact]
        public void SerializableOrderInvalidException()
        {
            var x = new OrderInvalidException("foo");
            Assert.DoesNotThrow(() => {
                    var y = SerializeAndCompare(x);
                    Assert.Equal(x.ErrorMessage, y.ErrorMessage);
            });
        }

        [Fact]
        public void SerializableOrderRejectedException()
        {
            var x = new OrderRejectedException("foo");
            Assert.DoesNotThrow(() => {
                    var y = SerializeAndCompare(x);
                    Assert.Equal(x.ErrorMessage, y.ErrorMessage);
            });
        }

        [Fact]
        public void SerializableReceiverDeadlockException()
        {
            var x = new ReceiverDeadlockException("foo");
            Assert.DoesNotThrow(() => {
                    var y = SerializeAndCompare(x);
                    Assert.Equal(x.ErrorMessage, y.ErrorMessage);
            });
        }

        [Fact]
        public void SerializableRequestTimedOutException()
        {
            var x = new RequestTimedOutException(60);
            Assert.DoesNotThrow(() => {
                    var y = SerializeAndCompare(x);
                    Assert.Equal(x.Timeout, y.Timeout);
            });
        }
    }
}
