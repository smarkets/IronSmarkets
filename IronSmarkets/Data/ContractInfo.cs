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

using System.Collections.Generic;

namespace IronSmarkets.Data
{
    public class ContractInfo
    {
        private static readonly IDictionary<Proto.Seto.ContractType, string> TypeStrings =
            new Dictionary<Proto.Seto.ContractType, string> {
            { Proto.Seto.ContractType.CONTRACTHALFTIMEFULLTIME, "half-time-full-time" },
            { Proto.Seto.ContractType.CONTRACTCORRECTSCORE, "correct-score" },
            { Proto.Seto.ContractType.CONTRACTGENERIC, "generic" },
            { Proto.Seto.ContractType.CONTRACTWINNER, "winner" },
            { Proto.Seto.ContractType.CONTRACTBINARY, "binary" },
            { Proto.Seto.ContractType.CONTRACTOVERUNDER, "over-under" }
        };

        private readonly Uuid _uuid;
        private readonly string _type;
        private readonly string _slug;
        private readonly string _name;
        private readonly string _shortname;
        private readonly IEnumerable<KeyValuePair<Uuid, string>> _entities;

        public Uuid Uuid { get { return _uuid; } }
        public string Type { get { return _type; } }
        public string Slug { get { return _slug; } }
        public string Name { get { return _name; } }
        public string Shortname { get { return _shortname; } }
        public IEnumerable<KeyValuePair<Uuid, string>> Entities { get { return _entities; } }

        private ContractInfo(
            Uuid uuid,
            string type,
            string slug,
            string name,
            string shortname,
            IEnumerable<KeyValuePair<Uuid, string>> entities)
        {
            _uuid = uuid;
            _type = type;
            _slug = slug;
            _name = name;
            _shortname = shortname;
            _entities = entities;
        }

        internal static ContractInfo FromSeto(Proto.Seto.ContractInfo info)
        {
            return new ContractInfo(
                Uuid.FromUuid128(info.Contract),
                TypeStrings[info.Type],
                info.Slug,
                info.Name,
                info.Shortname,
                EntityRelationships.FromEntities(info.Entities));
        }
    }
}
