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
using System.Collections.Generic;

using IronSmarkets.Proto.Seto;

namespace IronSmarkets.Data
{
    public class Event
    {
        internal static readonly IDictionary<EventCategory, string> CategoryStrings =
            new Dictionary<EventCategory, string> {
            { EventCategory.EVENTCATEGORYSPORT, "sport" },
            { EventCategory.EVENTCATEGORYPOLITICS, "politics" },
            { EventCategory.EVENTCATEGORYCURRENTAFFAIRS, "current-affairs" },
            { EventCategory.EVENTCATEGORYTVANDENTERTAINMENT, "tv-and-entertainment" },
            { EventCategory.EVENTCATEGORYGENERIC, "generic" }
        };

        internal static readonly IDictionary<string, EventCategory> Categories =
            new Dictionary<string, EventCategory> {
            { "politics", EventCategory.EVENTCATEGORYPOLITICS },
            { "current-affairs", EventCategory.EVENTCATEGORYCURRENTAFFAIRS },
            { "tv-and-entertainment", EventCategory.EVENTCATEGORYTVANDENTERTAINMENT },
            { "sport", EventCategory.EVENTCATEGORYSPORT },
            { "generic", EventCategory.EVENTCATEGORYGENERIC }
        };

        internal static readonly IDictionary<string, SportByDateType> Sports =
            new Dictionary<string, SportByDateType> {
            { "football", SportByDateType.SPORTBYDATEFOOTBALL },
            { "horse-racing", SportByDateType.SPORTBYDATEHORSERACING },
            { "tennis", SportByDateType.SPORTBYDATETENNIS }
        };

        internal static readonly IDictionary<EventType, string> TypeStrings =
            new Dictionary<EventType, string> {
            { EventType.EVENTFOOTBALLMATCH, "football-match" },
            { EventType.EVENTFOOTBALLSEASON, "football-season" },
            { EventType.EVENTFOOTBALL, "football" },
            { EventType.EVENTGENERIC, "generic" },
            { EventType.EVENTFOOTBALLGENERIC, "football-generic" },
            { EventType.EVENTGOLFSEASON, "golf-season" },
            { EventType.EVENTBOXINGSEASON, "boxing-season" },
            { EventType.EVENTFORMULA1RACE, "formula-1-race" },
            { EventType.EVENTFORMULA1SEASON, "formula-1-season" },
            { EventType.EVENTHORSERACINGRACE, "horse-racing-race" },
            { EventType.EVENTHORSERACINGCOURSE, "horse-racing-course" },
            { EventType.EVENTHORSERACING, "horse-racing" },
            { EventType.EVENTGOLFGENERIC, "golf-generic" },
            { EventType.EVENTEUROVISIONSEASON, "eurovision-season" },
            { EventType.EVENTTENNISMATCH, "tennis-match" },
            { EventType.EVENTTENNISSEASON, "tennis-season" },
            { EventType.EVENTCYCLINGSEASON, "cycling-season" },
            { EventType.EVENTCYCLINGRACE, "cycling-race" },
            { EventType.EVENTMOTOGPSEASON, "motogp-season" }
        };

        private readonly Uuid _uuid;
        private readonly string _name;
        private readonly string _type;
        private readonly string _category;
        private readonly string _slug;
        private readonly Uuid? _parentUuid;
        private readonly DateTime? _startDateTime;
        private readonly DateTime? _endDateTime;
        private readonly string _description;
        private readonly IEnumerable<KeyValuePair<Uuid, string>> _entities;
        private readonly IDictionary<Uuid, Market> _markets;

        public Uuid Uuid { get { return _uuid; } }
        public string Name { get { return _name; } }
        public string Type { get { return _type; } }
        public string Category { get { return _category; } }
        public string Slug { get { return _slug; } }
        public Uuid? ParentUuid { get { return _parentUuid; } }
        public DateTime? StartDateTime { get { return _startDateTime; } }
        public DateTime? EndDateTime { get { return _endDateTime; } }
        public string Description { get { return _description; } }
        public IEnumerable<KeyValuePair<Uuid, string>> Entities { get { return _entities; } }

        private Event(
            Uuid uuid,
            string name,
            string type,
            string category,
            string slug,
            Uuid? parentUuid,
            DateTime? startDateTime,
            DateTime? endDateTime,
            string description,
            IEnumerable<KeyValuePair<Uuid, String>> entities,
            IDictionary<Uuid, Market> markets)
        {
            _uuid = uuid;
            _name = name;
            _type = type;
            _category = category;
            _slug = slug;
            _parentUuid = parentUuid;
            _startDateTime = startDateTime;
            _endDateTime = endDateTime;
            _description = description;
            _entities = entities;
            _markets = markets;
        }

        internal static Event FromSeto(EventInfo info)
        {
            return new Event(
                Uuid.FromUuid128(info.Event),
                info.Name,
                TypeStrings[info.Type],
                CategoryStrings[info.Category],
                info.Slug,
                Uuid.MaybeFromUuid128(info.Parent),
                SetoMap.FromDateTime(info.StartDate, info.StartTime),
                SetoMap.FromDateTime(info.EndDate, info.EndTime),
                info.Description,
                EntityRelationships.FromEntities(info.Entities),
                MarketMap.FromMarkets(info.Markets));
        }
    }
}
