using System.IO;
using System.Linq;
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
        private dynamic _serialized;

        public JsonLdSerializationSteps(SerializationContext context)
        {
            _context = context;
            _serializer = new JsonLdSerializer(_context.ContextProvider);
        }

        [When(@"model is serialized")]
        public void WhenModelIsSerializedTo()
        {
            _serializer.Serialize(RdfSerialization.JsonLd.MediaType, ScenarioContext.Current["model"], _context.OutputStream);

            _context.OutputStream.Seek(0, SeekOrigin.Begin);
            using (var streamReader = new StreamReader(_context.OutputStream))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    _serialized = JToken.ReadFrom(jsonTextReader);
                }
            }
        }

        [Then(@"json object should contain key '(.*)' with value '(.*)'")]
        public void ThenJsonObjectShouldContainKeyWithValue(string key, string value)
        {
            Assert.That((string)_serialized[key], Is.EqualTo(value));
        }

        [Then(@"json object should not contain key '(.*)'")]
        public void ThenJsonObjectShouldNotContainKey(string key)
        {
            Assert.That(_serialized[key], Is.Null);
        }

        [Then(@"@types property should contain")]
        public void ThenTypesPropertyShouldContain(Table table)
        {
            JArray types = _serialized["@type"];
            foreach (var type in table.Rows.Select(row => new JValue(row[0])))
            {
                Assert.That(types, Contains.Item(type));
            }
        }
    }
}
