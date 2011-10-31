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
using System.Linq;

using IronSmarkets.Proto.Seto;

namespace IronSmarkets.Data
{
    internal abstract class EntityRelationships
    {
        private static readonly IDictionary<EntityRelationshipType, string> EntityRelationshipStrings =
            new Dictionary<EntityRelationshipType, string>
            {
                { EntityRelationshipType.ENTITYRELATIONSHIPFOOTBALLHOMETEAM, "football-home-team" },
                { EntityRelationshipType.ENTITYRELATIONSHIPFOOTBALLAWAYTEAM, "football-away-team" },
                { EntityRelationshipType.ENTITYRELATIONSHIPFOOTBALLCOMPETITION, "football-competition" },
                { EntityRelationshipType.ENTITYRELATIONSHIPHORSERACINGCOURSE, "horse-racing-course" },
                { EntityRelationshipType.ENTITYRELATIONSHIPHORSERACINGHORSE, "horse-racing-horse" },
                { EntityRelationshipType.ENTITYRELATIONSHIPHORSERACINGJOCKEY, "horse-racing-jockey" },
                { EntityRelationshipType.ENTITYRELATIONSHIPCONTRACTASSOCIATED, "contract-associated" },
                { EntityRelationshipType.ENTITYRELATIONSHIPGENERIC, "generic" },
                { EntityRelationshipType.ENTITYRELATIONSHIPTENNISPLAYERA, "tennis-player-a" },
                { EntityRelationshipType.ENTITYRELATIONSHIPTENNISPLAYERB, "tennis-player-b" },
                { EntityRelationshipType.ENTITYRELATIONSHIPHOMETEAM, "home-team" },
                { EntityRelationshipType.ENTITYRELATIONSHIPAWAYTEAM, "away-team" }
            };

        public static IEnumerable<KeyValuePair<Uuid, string>> FromEntities(IEnumerable<EntityRelationship> entities)
        {
            return entities.Select(
                entity => new KeyValuePair<Uuid, string>(
                    Uuid.FromUuid128(entity.Entity),
                    EntityRelationshipStrings[entity.Relationship]));
        }
    }
}
