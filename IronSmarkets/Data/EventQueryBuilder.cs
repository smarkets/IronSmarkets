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

using IronSmarkets.Proto.Seto;

namespace IronSmarkets.Data
{
    public sealed class EventQueryBuilder
    {
        private EventCategory _category;
        private SportByDateType _sport;
        private DateTime? _dateTime;

        public void SetCategory(string category)
        {
            if (!Event.Categories.TryGetValue(category, out _category))
            {
                throw new ArgumentException("Invalid category.");
            }
        }

        public void SetSport(string sport)
        {
            if (!Event.Sports.TryGetValue(sport, out _sport))
            {
                throw new ArgumentException("Invalid sport.");
            }
        }

        public void SetDateTime(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public EventQuery GetResult()
        {
            if (_category != EventCategory.EVENTCATEGORYSPORT && _dateTime.HasValue)
            {
                throw new ArgumentException(
                    "Date can only be specified for football, " +
                    "horse racing, and tennis.");
            }

            var request = new EventsRequest {
                ContentType = ContentType.CONTENTTYPEPROTOBUF
            };

            if (!_dateTime.HasValue)
            {
                if (_category == EventCategory.EVENTCATEGORYPOLITICS)
                {
                    request.Type = EventsRequestType.EVENTSREQUESTPOLITICS;
                }
                else if (_category == EventCategory.EVENTCATEGORYCURRENTAFFAIRS)
                {
                    request.Type = EventsRequestType.EVENTSREQUESTCURRENTAFFAIRS;
                }
                else if (_category == EventCategory.EVENTCATEGORYTVANDENTERTAINMENT)
                {
                    request.Type = EventsRequestType.EVENTSREQUESTTVANDENTERTAINMENT;
                }
                else
                {
                    request.Type = EventsRequestType.EVENTSREQUESTSPORTOTHER;
                }
            }
            else
            {
                var dateTime = _dateTime.GetValueOrDefault();
                request.Type = EventsRequestType.EVENTSREQUESTSPORTBYDATE;
                request.SportByDate = new SportByDate {
                    Type = _sport,
                    Date = new Date {
                        Year = (uint)dateTime.Year,
                        Month = (uint)dateTime.Month,
                        Day = (uint)dateTime.Day
                    }
                };
            }

            return new EventQuery(request);
        }
    }
}
