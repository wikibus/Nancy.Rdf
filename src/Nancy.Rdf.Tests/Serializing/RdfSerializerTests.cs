using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FakeItEasy;
using JsonLD.Entities;
using Nancy.Rdf.Responses;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using VDS.RDF;
using Vocab;

namespace Nancy.Rdf.Tests.Serializing
{
    [TestFixture]
    public class RdfSerializerTests
    {
        private RdfSerializerTestable _serializer;
        private IEntitySerializer _entitySerializer;
        private INodeFactory _nodeFactory = new NodeFactory();

        [SetUp]
        public void Setup()
        {
            _entitySerializer = A.Fake<IEntitySerializer>();
            _serializer = new RdfSerializerTestable(_entitySerializer);
        }

        [Test]
        public void Should_serialize_bool_with_proper_format()
        {
            // given
            A.CallTo(() => _entitySerializer.Serialize(A<object>._, A<SerializationOptions>._))
                .Returns(JObject.Parse(@"{
  '@context':
    {
      'dateCreated': 'http://example.api/o#Issue/dateCreated',
      'isResolved': 'http://example.api/o#Issue/isResolved'
    },
  '@id': 'http://localhost:61186/issues/2',
  'dateCreated': '2016-03-21T00:00:00',
  'isResolved': true
}"));

            // when
            _serializer.Serialize("text/turtle", new object(), new MemoryStream());

            // then
            Assert.That(_serializer.Triples.Any(triple =>
            {
                var valueMatches = ((LiteralNode)triple.Object).Value == "true";
                var dataTypeMatches = ((LiteralNode)triple.Object).DataType == new Uri(Xsd.boolean);

                return valueMatches && dataTypeMatches;
            }));
        }

        [Test]
        public void Should_serialize_datetime_with_proper_format()
        {
            // given
            A.CallTo(() => _entitySerializer.Serialize(A<object>._, A<SerializationOptions>._))
                .Returns(JObject.Parse(@"{
  '@context':
    {
      'dateCreated': 'http://example.api/o#Issue/dateCreated',
      'isResolved': 'http://example.api/o#Issue/isResolved'
    },
  '@id': 'http://localhost:61186/issues/2',
  'dateCreated': { '@value': '2016-03-21T00:00:00', '@type': 'http://www.w3.org/2001/XMLSchema#dateTime' },
  'isResolved': true
}"));

            // when
            _serializer.Serialize("text/turtle", new object(), new MemoryStream());

            // then
            Assert.That(_serializer.Triples.Any(triple =>
            {
                var valueMatches = ((LiteralNode)triple.Object).Value == "2016-03-21T00:00:00";
                var dataTypeMatches = ((LiteralNode)triple.Object).DataType == new Uri(Xsd.dateTime);

                return valueMatches && dataTypeMatches;
            }));
        }

        [Test]
        public void Should_serialize_correct_relative_URIs_according_to_base()
        {
            // given
            A.CallTo(() => _entitySerializer.Serialize(A<object>._, A<SerializationOptions>._))
                .Returns(JObject.Parse(@"{
    '@id': 'some/id',
    'http://example.com/property': { '@id': 'some/relative/path' }
}"));

            // when
            _serializer.Serialize("text/turtle", new WrappedModel(new object(), "http://example.api/site"), new MemoryStream());

            // then
            Assert.That(_serializer.Triples.Contains(new Triple(
                _nodeFactory.CreateUriNode(new Uri("http://example.api/site/some/id")),
                _nodeFactory.CreateUriNode(new Uri("http://example.com/property")),
                _nodeFactory.CreateUriNode(new Uri("http://example.api/site/some/relative/path")))));
        }

        private class RdfSerializerTestable : RdfSerializer
        {
            public RdfSerializerTestable(IEntitySerializer entitySerializer)
                : base(RdfSerialization.Turtle, entitySerializer)
            {
                Triples = new List<Triple>();
            }

            public IList<Triple> Triples { get; set; }

            protected override void WriteRdf(StreamWriter writer, IEnumerable<Triple> triples)
            {
                Triples = triples.ToList();
            }
        }
    }
}
