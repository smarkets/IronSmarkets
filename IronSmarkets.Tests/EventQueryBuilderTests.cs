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

using Xunit;

using IronSmarkets.Data;
using IronSmarkets.Exceptions;

namespace IronSmarkets.Tests
{
    public sealed class EventQueryBuilderTests
    {
        [Fact]
        public void ThrowsOnInvalidCategory()
        {
            Assert.Throws<ArgumentException>(() => new EventQueryBuilder().SetCategory("foo"));
            Assert.Throws<InvalidEventQueryException>(() => new EventQueryBuilder().GetResult().ToEventsRequest());
        }

        [Fact]
        public void ThrowsOnInvalidSport()
        {
            Assert.Throws<ArgumentException>(() => new EventQueryBuilder().SetSport("foo"));
            var builder = new EventQueryBuilder();
            builder.SetSport("football");
            // Need to also specify category of sport
            Assert.Throws<InvalidEventQueryException>(() => builder.GetResult());

            // If we only specify sport/football, we still need a date
            builder.SetCategory("sport");
            Assert.Throws<InvalidEventQueryException>(() => builder.GetResult());

            builder.SetDateTime(new DateTime(2012, 1, 1));
            Assert.DoesNotThrow(() => builder.GetResult());
        }

        [Fact]
        public void ContentType()
        {
            var builder = new EventQueryBuilder();
            builder.SetCategory("sport");
            Assert.Equal(builder.GetResult().ToEventsRequest().ContentType, Proto.Seto.ContentType.CONTENTTYPEPROTOBUF);
        }

        [Fact]
        public void EventsRequestType()
        {
            var builder = new EventQueryBuilder();
            builder.SetCategory("politics");
            var result = builder.GetResult().ToEventsRequest();
            Assert.Equal(result.Type, Proto.Seto.EventsRequestType.EVENTSREQUESTPOLITICS);
            builder.SetCategory("current-affairs");
            result = builder.GetResult().ToEventsRequest();
            Assert.Null(result.SportByDate);
            Assert.Equal(result.Type, Proto.Seto.EventsRequestType.EVENTSREQUESTCURRENTAFFAIRS);
            builder.SetCategory("tv-and-entertainment");
            result = builder.GetResult().ToEventsRequest();
            Assert.Null(result.SportByDate);
            Assert.Equal(result.Type, Proto.Seto.EventsRequestType.EVENTSREQUESTTVANDENTERTAINMENT);
            builder.SetCategory("sport");
            result = builder.GetResult().ToEventsRequest();
            Assert.Null(result.SportByDate);
            Assert.Equal(result.Type, Proto.Seto.EventsRequestType.EVENTSREQUESTSPORTOTHER);
            builder.SetDateTime(new DateTime(2012, 1, 1));
            Assert.Throws<InvalidEventQueryException>(() => builder.GetResult());
            builder.SetSport("football");
            result = builder.GetResult().ToEventsRequest();
            Assert.Equal(result.Type, Proto.Seto.EventsRequestType.EVENTSREQUESTSPORTBYDATE);
            Assert.NotNull(result.SportByDate);
        }

        [Fact]
        public void SportByDateType()
        {
            var builder = new EventQueryBuilder();
            builder.SetCategory("sport");
            builder.SetDateTime(new DateTime(2012, 1, 1));
            builder.SetSport("football");
            Assert.Equal(builder.GetResult().ToEventsRequest().SportByDate.Type, Proto.Seto.SportByDateType.SPORTBYDATEFOOTBALL);
            builder.SetSport("tennis");
            Assert.Equal(builder.GetResult().ToEventsRequest().SportByDate.Type, Proto.Seto.SportByDateType.SPORTBYDATETENNIS);
            builder.SetSport("horse-racing");
            Assert.Equal(builder.GetResult().ToEventsRequest().SportByDate.Type, Proto.Seto.SportByDateType.SPORTBYDATEHORSERACING);

            var builderDate = builder.GetResult().ToEventsRequest().SportByDate.Date;
            Assert.Equal(builderDate.Year, (uint)2012);
            Assert.Equal(builderDate.Month, (uint)1);
            Assert.Equal(builderDate.Day, (uint)1);
        }
    }
}
