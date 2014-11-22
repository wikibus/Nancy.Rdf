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
        private readonly JsonLdSerializationContext _context;

        public JsonLdSerializationSteps(JsonLdSerializationContext context)
        {
            _context = context;
            _serializer = new JsonLdSerializer(context.ContextProvider);
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
                    _context.Serialized = JToken.ReadFrom(jsonTextReader);
                }
            }
        }

        [Then(@"json object should contain key '(.*)' with value '(.*)'")]
        public void ThenJsonObjectShouldContainKeyWithValue(string key, string value)
        {
            Assert.That((string)_context.Serialized[key], Is.EqualTo(value));
        }

        [Then(@"json object should not contain key '(.*)'")]
        public void ThenJsonObjectShouldNotContainKey(string key)
        {
            Assert.That(_context.Serialized[key], Is.Null);
        }
    }
}
