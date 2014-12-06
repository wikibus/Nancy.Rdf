using System.Collections.Generic;
using Nancy.RDF.Responses;
using Nancy.Responses.Negotiation;
using NUnit.Framework;

namespace Nancy.RDF.Tests
{
    [TestFixture]
    public class RdfResponseProcessorTests
    {
        [Test]
        public void Should_not_allow_processing_when_no_comatible_serializer_is_available()
        {
            // given
            var serializers = new ISerializer[0];
            var processor = new RdfResponseProcessorTestable(serializers);

            // when
            var match = processor.CanProcess(new MediaRange(RdfSerialization.RdfXml.MediaType), new object(), new NancyContext());

            // then
            Assert.That(match.ModelResult, Is.EqualTo(MatchResult.DontCare));
            Assert.That(match.RequestedContentTypeResult, Is.EqualTo(MatchResult.NoMatch));
        }

        private class RdfResponseProcessorTestable : RdfResponseProcessor
        {
            public RdfResponseProcessorTestable(IEnumerable<ISerializer> serializers)
                : base(RdfSerialization.RdfXml, serializers)
            {
            }
        }
    }
}
