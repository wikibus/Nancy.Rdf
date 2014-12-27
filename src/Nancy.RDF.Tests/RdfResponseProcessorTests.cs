using System.Collections.Generic;
using FakeItEasy;
using Nancy.RDF.Responses;
using Nancy.Responses.Negotiation;
using NUnit.Framework;

namespace Nancy.RDF.Tests
{
    [TestFixture]
    public class RdfResponseProcessorTests
    {
        [Test]
        public void Should_not_allow_processing_when_no_compatible_serializer_is_available()
        {
            // given
            var serializers = new ISerializer[0];
            var processor = new RdfResponseProcessorTestable(serializers, new RdfResponseOptions());

            // when
            var match = processor.CanProcess(new MediaRange(RdfSerialization.RdfXml.MediaType), new object(), new NancyContext());

            // then
            Assert.That(match.ModelResult, Is.EqualTo(MatchResult.DontCare));
            Assert.That(match.RequestedContentTypeResult, Is.EqualTo(MatchResult.NoMatch));
        }

        [Test]
        public void Should_match_wildcard_when_set_up()
        {
            // given
            var serializer = A.Fake<ISerializer>();
            A.CallTo(() => serializer.CanSerialize(A<string>.Ignored)).Returns(true);
            var options = new RdfResponseOptions
            {
                FallbackSerialization = RdfSerialization.RdfXml
            };
            var processor = new RdfResponseProcessorTestable(new[] { serializer }, options);

            // when
            var match = processor.CanProcess(new MediaRange("*/*"), new object(), new NancyContext());

            // then
            Assert.That(match.ModelResult, Is.EqualTo(MatchResult.DontCare));
            Assert.That(match.RequestedContentTypeResult, Is.EqualTo(MatchResult.NonExactMatch));
        }

        [Test]
        public void Should_not_match_wildcard_when_another_fallback_set_up()
        {
            // given
            var serializer = A.Fake<ISerializer>();
            A.CallTo(() => serializer.CanSerialize(A<string>.Ignored)).Returns(true);
            var options = new RdfResponseOptions
            {
                FallbackSerialization = RdfSerialization.Turtle
            };
            var processor = new RdfResponseProcessorTestable(new[] { serializer }, options);

            // when
            var match = processor.CanProcess(new MediaRange("*/*"), new object(), new NancyContext());

            // then
            Assert.That(match.ModelResult, Is.EqualTo(MatchResult.DontCare));
            Assert.That(match.RequestedContentTypeResult, Is.EqualTo(MatchResult.NoMatch));
        }

        [Test]
        public void Should_not_match_wildcard_when_not_set_up()
        {
            // given
            var serializer = A.Fake<ISerializer>();
            A.CallTo(() => serializer.CanSerialize(A<string>.Ignored)).Returns(true);
            var processor = new RdfResponseProcessorTestable(new[] { serializer }, new RdfResponseOptions());

            // when
            var match = processor.CanProcess(new MediaRange("*/*"), new object(), new NancyContext());

            // then
            Assert.That(match.ModelResult, Is.EqualTo(MatchResult.DontCare));
            Assert.That(match.RequestedContentTypeResult, Is.EqualTo(MatchResult.NoMatch));
        }

        private class RdfResponseProcessorTestable : RdfResponseProcessor
        {
            public RdfResponseProcessorTestable(IEnumerable<ISerializer> serializers, RdfResponseOptions options)
                : base(RdfSerialization.RdfXml, serializers, options)
            {
            }
        }
    }
}
