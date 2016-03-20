using System;
using JsonLD.Entities.Context;
using Microsoft.Owin;
using Nancy.Rdf.Sample;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Owin;
using Vocab;

[assembly: OwinStartup(typeof(Startup))]

namespace Nancy.Rdf.Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }

    public class Person
    {
        public string Id { get; set; }

        [JsonProperty("givenName")]
        public string Name { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [JsonProperty]
        private string Type
        {
            get
            {
                return Foaf.Person;
            }
        }

        private static JObject Context
        {
            get
            {
                return new JObject(
                  "givenName".IsProperty(Foaf.givenName),
                  "lastName".IsProperty(Foaf.lastName),
                  "dateOfBirth".IsProperty(Schema.birthDate)
                );
            }
        }
    }
    public class PersonModule : NancyModule
    {
        public PersonModule()
        {
            Get["person/{id}"] = _ =>
            {
                return new Person
                {
                    Id = "http://api.guru/person/" + _.id,
                    Name = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1967, 8, 2)
                };
            };
        }
    }

    public class ContextPathMapper : Contexts.DefaultContextPathMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextPathMapper"/> class.
        /// </summary>
        public ContextPathMapper()
        {
            ServeContextOf<Person>();
        }
    }
}