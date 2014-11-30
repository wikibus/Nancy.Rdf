using System;
using FakeItEasy;
using Nancy.Responses.Negotiation;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Nancy.RDF.Tests.Bindings
{
    [Binding]
    public class ContentNegotiationSteps
    {
        private readonly ISerializer _serializer;
        private MediaRange _mediaRange;
        private Response _response;

        public ContentNegotiationSteps()
        {
            _serializer = A.Fake<ISerializer>();
            A.CallTo(() => _serializer.CanSerialize(A<string>._)).Returns(true);
        }

        [Given(@"requested media range '(.*)'")]
        public void GivenRequestedMediaRange(string mediaRange)
        {
            _mediaRange = new MediaRange(mediaRange);
        }

        [When(@"processing model")]
        public void WhenProcessingRdfModel()
        {
            var processor = new Responses.RdfResponseProcessor(new[] { _serializer });

            _response = processor.Process(_mediaRange, new object(), new NancyContext());
        }

        [When(@"processing model"), Scope(Tag = "JsonLd")]
        public void WhenProcessingModel()
        {
            var processor = new Responses.JsonLdResponseProcessor(new[] { _serializer });

            _response = processor.Process(_mediaRange, new object(), new NancyContext());
        }

        [Then(@"response should have status '(.*)'")]
        public void ThenResponseShouldHaveStatus(string expectedStatusCode)
        {
            Assert.That(_response.StatusCode, Is.EqualTo(Enum.Parse(typeof(HttpStatusCode), expectedStatusCode)));
        }
    }
}
