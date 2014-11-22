using System.IO;
using Nancy.RDF.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Nancy.RDF.Tests.Bindings
{
    [Binding, Scope(Tag = "JsonLd")]
    public class JsonLdSerializationSteps
    {
        private readonly ISerializer _serializer;
        private readonly SerializationContext _context;

        public JsonLdSerializationSteps(SerializationContext context)
        {
            _context = context;
            _serializer = new JsonLdSerializer(context.ContextProvider);
        }

        [When(@"model is serialized")]
        public void WhenModelIsSerializedTo(string rdfSerialization)
        {
            _serializer.Serialize(rdfSerialization, ScenarioContext.Current["model"], _context.OutputStream);
        }

        [Then(@"json object should contain key '(.*)' with value '(.*)'")]
        public void ThenJsonObjectShouldContainKeyWithValue(string key, string value)
        {
            _context.OutputStream.Seek(0, SeekOrigin.Begin);

            dynamic serialized;
            using (var streamReader = new StreamReader(_context.OutputStream))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    serialized = JToken.ReadFrom(jsonTextReader);
                }
            }

            Assert.That((string)serialized[key], Is.EqualTo(value));
        }
    }
}
