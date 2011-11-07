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

namespace IronSmarkets.Data
{
    public class EventInfo
    {
        private static readonly IDictionary<Proto.Seto.EventCategory, string> CategoryStrings =
            new Dictionary<Proto.Seto.EventCategory, string> {
            { Proto.Seto.EventCategory.EVENTCATEGORYSPORT, "sport" },
            { Proto.Seto.EventCategory.EVENTCATEGORYPOLITICS, "politics" },
            { Proto.Seto.EventCategory.EVENTCATEGORYCURRENTAFFAIRS, "current-affairs" },
            { Proto.Seto.EventCategory.EVENTCATEGORYTVANDENTERTAINMENT, "tv-and-entertainment" },
            { Proto.Seto.EventCategory.EVENTCATEGORYGENERIC, "generic" },
            { Proto.Seto.EventCategory.EVENTCATEGORYFOOTBALL, "football" },
            { Proto.Seto.EventCategory.EVENTCATEGORYTENNIS, "tennis" },
            { Proto.Seto.EventCategory.EVENTCATEGORYHORSERACING, "horse-racing" },
            { Proto.Seto.EventCategory.EVENTCATEGORYAMERICANFOOTBALL, "american-football" },
            { Proto.Seto.EventCategory.EVENTCATEGORYRUGBY, "rugby" }
        };

        internal static readonly IDictionary<string, Proto.Seto.EventCategory> Categories =
            new Dictionary<string, Proto.Seto.EventCategory> {
            { "politics", Proto.Seto.EventCategory.EVENTCATEGORYPOLITICS },
            { "current-affairs", Proto.Seto.EventCategory.EVENTCATEGORYCURRENTAFFAIRS },
            { "tv-and-entertainment", Proto.Seto.EventCategory.EVENTCATEGORYTVANDENTERTAINMENT },
            { "sport", Proto.Seto.EventCategory.EVENTCATEGORYSPORT },
            { "generic", Proto.Seto.EventCategory.EVENTCATEGORYGENERIC },
            { "football", Proto.Seto.EventCategory.EVENTCATEGORYFOOTBALL },
            { "tennis", Proto.Seto.EventCategory.EVENTCATEGORYTENNIS },
            { "horse-racing", Proto.Seto.EventCategory.EVENTCATEGORYHORSERACING },
            { "american-football", Proto.Seto.EventCategory.EVENTCATEGORYAMERICANFOOTBALL },
            { "rugby", Proto.Seto.EventCategory.EVENTCATEGORYRUGBY }
        };

        internal static readonly IDictionary<string, Proto.Seto.SportByDateType> Sports =
            new Dictionary<string, Proto.Seto.SportByDateType> {
            { "football", Proto.Seto.SportByDateType.SPORTBYDATEFOOTBALL },
            { "horse-racing", Proto.Seto.SportByDateType.SPORTBYDATEHORSERACING },
            { "tennis", Proto.Seto.SportByDateType.SPORTBYDATETENNIS }
        };

        private static readonly IDictionary<Proto.Seto.EventType, string> TypeStrings =
            new Dictionary<Proto.Seto.EventType, string> {
            { Proto.Seto.EventType.EVENTFOOTBALLMATCH, "football-match" },
            { Proto.Seto.EventType.EVENTFOOTBALLSEASON, "football-season" },
            { Proto.Seto.EventType.EVENTFOOTBALL, "football" },
            { Proto.Seto.EventType.EVENTGENERIC, "generic" },
            { Proto.Seto.EventType.EVENTFOOTBALLGENERIC, "football-generic" },
            { Proto.Seto.EventType.EVENTGOLFSEASON, "golf-season" },
            { Proto.Seto.EventType.EVENTBOXINGSEASON, "boxing-season" },
            { Proto.Seto.EventType.EVENTFORMULA1RACE, "formula-1-race" },
            { Proto.Seto.EventType.EVENTFORMULA1SEASON, "formula-1-season" },
            { Proto.Seto.EventType.EVENTHORSERACINGRACE, "horse-racing-race" },
            { Proto.Seto.EventType.EVENTHORSERACINGCOURSE, "horse-racing-course" },
            { Proto.Seto.EventType.EVENTHORSERACING, "horse-racing" },
            { Proto.Seto.EventType.EVENTGOLFGENERIC, "golf-generic" },
            { Proto.Seto.EventType.EVENTEUROVISIONSEASON, "eurovision-season" },
            { Proto.Seto.EventType.EVENTTENNISROUND, "tennis-round" },
            { Proto.Seto.EventType.EVENTTENNISFORMAT, "tennis-format" },
            { Proto.Seto.EventType.EVENTTENNISTOURNAMENT, "tennis-tournament" },
            { Proto.Seto.EventType.EVENTCYCLINGSEASON, "cycling-season" },
            { Proto.Seto.EventType.EVENTCYCLINGRACE, "cycling-race" },
            { Proto.Seto.EventType.EVENTMOTOGPSEASON, "motogp-season" },
            { Proto.Seto.EventType.EVENTBOXINGMATCH, "boxing-match" },
            { Proto.Seto.EventType.EVENTAMERICANFOOTBALLMATCH, "american-football-match" },
            { Proto.Seto.EventType.EVENTRUGBYUNIONMATCH, "rugby-union-match" }
        };

        private readonly Uid _uid;
        private readonly string _name;
        private readonly string _type;
        private readonly string _category;
        private readonly string _slug;
        private readonly Uid? _parentUid;
        private readonly DateTime? _startDateTime;
        private readonly DateTime? _endDateTime;
        private readonly string _description;
        private readonly IEnumerable<KeyValuePair<Uid, string>> _entities;

        public Uid Uid { get { return _uid; } }
        public string Name { get { return _name; } }
        public string Type { get { return _type; } }
        public string Category { get { return _category; } }
        public string Slug { get { return _slug; } }
        public Uid? ParentUid { get { return _parentUid; } }
        public DateTime? StartDateTime { get { return _startDateTime; } }
        public DateTime? EndDateTime { get { return _endDateTime; } }
        public string Description { get { return _description; } }
        public IEnumerable<KeyValuePair<Uid, string>> Entities { get { return _entities; } }

        private EventInfo(
            Uid uid,
            string name,
            string type,
            string category,
            string slug,
            Uid? parentUid,
            DateTime? startDateTime,
            DateTime? endDateTime,
            string description,
            IEnumerable<KeyValuePair<Uid, String>> entities)
        {
            _uid = uid;
            _name = name;
            _type = type;
            _category = category;
            _slug = slug;
            _parentUid = parentUid;
            _startDateTime = startDateTime;
            _endDateTime = endDateTime;
            _description = description;
            _entities = entities;
        }

        internal static EventInfo FromSeto(Proto.Seto.EventInfo info)
        {
            return new EventInfo(
                Uid.FromUuid128(info.Event),
                info.Name,
                TypeStrings[info.Type],
                CategoryStrings[info.Category],
                info.Slug,
                Uid.MaybeFromUuid128(info.Parent),
                SetoMap.FromDateTime(info.StartDate, info.StartTime),
                SetoMap.FromDateTime(info.EndDate, info.EndTime),
                info.Description,
                EntityRelationships.FromEntities(info.Entities));
        }
    }
}
