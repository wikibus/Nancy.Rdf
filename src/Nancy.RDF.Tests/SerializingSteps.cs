using System;
using System.IO;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using Nancy;
using Nancy.RDF.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using wikibus.tests.Nancy.Models;

namespace wikibus.tests.Nancy
{
    [Binding]
    public class SerializingToRdfSteps
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISerializer _jsonLdResializer;
        private readonly Stream _outputStream;
        private object _model;

        public SerializingToRdfSteps()
        {
            _outputStream = new MemoryStream();
            _contextProvider = A.Fake<IContextProvider>();
            _jsonLdResializer = new JsonLdSerializer(_contextProvider);
        }

        [Given(@"A model with content:")]
        public void GivenAModelOfType(Table table)
        {
            _model = table.CreateInstance<Brochure>();
        }

        [Given(@"@context is:"), Scope(Tag = "Brochure")]
        public void GivenContextForIs(string resource)
        {
            A.CallTo(() => _contextProvider.GetContext<Brochure>()).Returns(JToken.Parse(resource));
        }

        [When(@"model is serialized to '(.*)'")]
        public void WhenModelIsSerializedTo(string rdfSerialization)
        {
            ISerializer serializer = null;
            if (rdfSerialization == RdfSerialization.JsonLd.MediaType)
            {
                serializer = _jsonLdResializer;
            }

            serializer.Serialize(rdfSerialization, _model, _outputStream);
            _outputStream.Seek(0, SeekOrigin.Begin);
        }
        
        [Then(@"json object should contain key '(.*)' with value '(.*)'")]
        public void ThenJsonObjectShouldContainKeyWithValue(string key, string value)
        {
            dynamic serialized;
            using (var streamReader = new StreamReader(_outputStream))
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
