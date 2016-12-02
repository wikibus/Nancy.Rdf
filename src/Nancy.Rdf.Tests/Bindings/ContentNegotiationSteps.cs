using System;
using System.Collections.Generic;
using FakeItEasy;
using Nancy.Rdf.Responses;
using Nancy.Responses.Negotiation;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Nancy.Rdf.Tests.Bindings
{
    [Binding]
    public class ContentNegotiationSteps
    {
        private readonly ISerializer serializer;
        private MediaRange mediaRange;
        private Response response;

        public ContentNegotiationSteps()
        {
            this.serializer = A.Fake<ISerializer>();
            A.CallTo(() => this.serializer.CanSerialize(A<string>._)).Returns(true);
        }

        [Given(@"requested media range '(.*)'")]
        public void GivenRequestedMediaRange(string mediaRange)
        {
            this.mediaRange = new MediaRange(mediaRange);
        }

        [When(@"processing model"), Scope(Tag = "Rdf")]
        public void WhenProcessingRdfModel()
        {
            var processor = new RdfResponseProcessorTestable(new[] { this.serializer });

            this.response = processor.Process(this.mediaRange, new object(), new NancyContext());
        }

        [Then(@"response should have status '(.*)'")]
        public void ThenResponseShouldHaveStatus(string expectedStatusCode)
        {
            Assert.That(this.response.StatusCode, Is.EqualTo(Enum.Parse(typeof(HttpStatusCode), expectedStatusCode)));
        }

        private class RdfResponseProcessorTestable : RdfResponseProcessor
        {
            public RdfResponseProcessorTestable(IEnumerable<ISerializer> serializers)
                : base(RdfSerialization.Turtle, serializers)
            {
            }
        }
    }
}
