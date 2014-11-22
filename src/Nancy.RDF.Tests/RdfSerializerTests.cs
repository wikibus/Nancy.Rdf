////using System;
////using System.IO;
////using FakeItEasy;
////using FluentAssertions;
////using Nancy;
////using Nancy.RDF.Responses;
////using NUnit.Framework;
////using VDS.RDF;
////using VDS.RDF.Writing.Formatting;
////using wikibus.sources;
////using wikibus.tests.Nancy.Models;

////namespace wikibus.tests.Nancy
////{
////    [TestFixture]
////    public class RdfSerializerTests
////    {
////        private static readonly RdfSerialization Serialization = RdfSerialization.Turtle;
////        private IRdfHandler _handler;
////        private ISerializer _serializer;

////        [SetUp]
////        public void Setup()
////        {
////            _handler = A.Fake<IRdfHandler>();
////            _serializer = new RdfSerializerTestable(_handler);
////        }

////        [Test]
////        public void Should_serialize_simple_property_to_triple()
////        {
////            // given
////            Stream output = new MemoryStream();
////            const string title = "Jelcz 123";
////            var model = new Brochure
////                {
////                    Title = title
////                };

////            // when
////            _serializer.Serialize(Serialization.MediaType, model, output);

////            // then
////            A.CallTo(() => _handler.HandleTriple(A<Triple>.That.Matches(t =>
////                                                                        t.Subject is IBlankNode &&
////                                                                        t.Predicate.ToString() == "http://purl.org/dc/terms/title" &&
////                                                                        ((ILiteralNode)t.Object).Value == "Jelcz 123"))).MustHaveHappened();
////        }

////        [Test]
////        public void Should_acccept_given_mime()
////        {
////            // given
////            string mime = Serialization.MediaType;

////            // when
////            var canSerialize = _serializer.CanSerialize(mime);

////            // then
////            canSerialize.Should().BeTrue();
////        }

////        private class RdfSerializerTestable : RdfSerializer
////        {
////            private readonly IRdfHandler _handler;

////            public RdfSerializerTestable(IRdfHandler handler)
////                : base(Serialization)
////            {
////                _handler = handler;
////            }

////            protected override IRdfHandler CreateHandler<TModel>(StreamWriter writer)
////            {
////                return _handler;
////            }

////            protected override ITripleFormatter CreateFormatter()
////            {
////                throw new NotImplementedException();
////            }
////        }
////    }
////}
