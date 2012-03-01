// Copyright (c) 2011-2012 Smarkets Limited
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
using System.Linq;

namespace IronSmarkets.Data
{
    public class MarketInfo
    {
        private readonly Uid _uid;
        private readonly string _slug;
        private readonly string _name;
        private readonly string _shortname;
        private readonly DateTime? _startDateTime;
        private readonly DateTime? _endDateTime;
        private readonly IEnumerable<KeyValuePair<Uid, string>> _entities;
        private readonly IEnumerable<Uid> _contracts;

        public Uid Uid { get { return _uid; } }
        public string Slug { get { return _slug; } }
        public string Name { get { return _name; } }
        public string Shortname { get { return _shortname; } }
        public DateTime? StartDateTime { get { return _startDateTime; } }
        public DateTime? EndDateTime { get { return _endDateTime; } }
        public IEnumerable<KeyValuePair<Uid, string>> Entities { get { return _entities; } }
        public IEnumerable<Uid> Contracts { get { return _contracts; } }

        private MarketInfo(
            Uid uid,
            string slug,
            string name,
            string shortname,
            DateTime? startDateTime,
            DateTime? endDateTime,
            IEnumerable<KeyValuePair<Uid, string>> entities,
            IEnumerable<Uid> contracts)
        {
            _uid = uid;
            _slug = slug;
            _name = name;
            _shortname = shortname;
            _startDateTime = startDateTime;
            _endDateTime = endDateTime;
            _entities = entities;
            _contracts = contracts;
        }

        internal static MarketInfo FromSeto(Proto.Seto.MarketInfo info)
        {
            return new MarketInfo(
                Uid.FromUuid128(info.Market),
                info.Slug,
                info.Name,
                info.Shortname,
                SetoMap.FromDateTime(info.StartDate, info.StartTime),
                SetoMap.FromDateTime(info.EndDate, info.EndTime),
                EntityRelationships.FromEntities(info.Entities),
                from c in info.Contracts select Uid.FromUuid128(c.Contract));
        }
    }
}
