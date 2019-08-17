using System;
using JsonLD.Entities.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vocab;

namespace Nancy.Rdf.Sample
{
    public class Person
    {
        public string Id { get; set; }

        [JsonProperty("givenName")]
        public string Name { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Friend { get; set; }

        public Person Parent { get; set; }

        [JsonProperty]
        private string Type
        {
            get
            {
                return Foaf.Person;
            }
        }

        private static JToken Context
        {
            get
            {
                return new VocabContext<Person>("http://example.com/vocab#")
                    .MergeWith(new JObject(
                    "givenName".IsProperty(Foaf.givenName),
                    "lastName".IsProperty(Foaf.lastName),
                    "dateOfBirth".IsProperty(Schema.birthDate)
                ));
            }
        }
    }
}