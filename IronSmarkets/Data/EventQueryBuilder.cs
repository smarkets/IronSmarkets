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

namespace IronSmarkets.Data
{
    public sealed class EventQueryBuilder
    {
        private Proto.Seto.EventCategory _category;
        private Proto.Seto.SportByDateType _sport;
        private DateTime? _dateTime;

        public void SetCategory(string category)
        {
            if (!EventInfo.Categories.TryGetValue(category, out _category))
            {
                throw new ArgumentException("Invalid category.");
            }
        }

        public void SetSport(string sport)
        {
            if (!EventInfo.Sports.TryGetValue(sport, out _sport))
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
            if (_category != Proto.Seto.EventCategory.EVENTCATEGORYSPORT && _dateTime.HasValue)
            {
                throw new ArgumentException(
                    "Date can only be specified for football, " +
                    "horse racing, and tennis.");
            }

            var request = new Proto.Seto.EventsRequest {
                ContentType = Proto.Seto.ContentType.CONTENTTYPEPROTOBUF
            };

            if (!_dateTime.HasValue)
            {
                if (_category == Proto.Seto.EventCategory.EVENTCATEGORYPOLITICS)
                {
                    request.Type = Proto.Seto.EventsRequestType.EVENTSREQUESTPOLITICS;
                }
                else if (_category == Proto.Seto.EventCategory.EVENTCATEGORYCURRENTAFFAIRS)
                {
                    request.Type = Proto.Seto.EventsRequestType.EVENTSREQUESTCURRENTAFFAIRS;
                }
                else if (_category == Proto.Seto.EventCategory.EVENTCATEGORYTVANDENTERTAINMENT)
                {
                    request.Type = Proto.Seto.EventsRequestType.EVENTSREQUESTTVANDENTERTAINMENT;
                }
                else
                {
                    request.Type = Proto.Seto.EventsRequestType.EVENTSREQUESTSPORTOTHER;
                }
            }
            else
            {
                var dateTime = _dateTime.GetValueOrDefault();
                request.Type = Proto.Seto.EventsRequestType.EVENTSREQUESTSPORTBYDATE;
                request.SportByDate = new Proto.Seto.SportByDate {
                    Type = _sport,
                    Date = new Proto.Seto.Date {
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
