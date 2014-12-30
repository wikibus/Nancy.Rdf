using System;
using System.Collections.Generic;
using System.IO;
using FakeItEasy;
using JsonLD.Entities;
using Nancy.RDF.Responses;
using NUnit.Framework;
using VDS.RDF;

namespace Nancy.RDF.Tests
{
    [TestFixture]
    public class CompressingSerializerTests
    {
        private CompressingSerializer _serializer;
        private WriterSpy _writer;
        private INamespaceManager _mapper;

        [SetUp]
        public void Setup()
        {
            var entitySerialier = A.Fake<IEntitySerializer>();
            _writer = new WriterSpy();
            _mapper = A.Fake<INamespaceManager>();
            _serializer = new CompressingSerializerTestable(entitySerialier, _writer, _mapper);
        }

        [Test]
        public void Serialized_graph_should_use_namespaces_if_given()
        {
            // given
            IEnumerable<NamespaceMap> prefixes = new[]
            {
                new NamespaceMap("ex", new Uri("http://example.com")),
                new NamespaceMap("lol", new Uri("http://lol.com"))
            };
            A.CallTo(() => _mapper.GetEnumerator()).Returns(prefixes.GetEnumerator());

            // when
            _serializer.Serialize(RdfSerialization.Turtle.MediaType, new object(), new MemoryStream());

            // then
            Assert.That(_writer.Prefixes.Prefixes, Has.Count.EqualTo(2));
            Assert.That(_writer.Prefixes.HasNamespace("ex"));
            Assert.That(_writer.Prefixes.HasNamespace("lol"));
        }

        [Test]
        public void Serialized_graph_should_use_base_uri_if_given()
        {
            // given
            var baseUri = new Uri("http://example.org/base");
            A.CallTo(() => _mapper.BaseUri).Returns(baseUri);

            // when
            _serializer.Serialize(RdfSerialization.Turtle.MediaType, new object(), new MemoryStream());

            // then
            Assert.That(_writer.BaseUri, Is.EqualTo(baseUri));
        }

        private class WriterSpy : IRdfWriter
        {
            public event RdfWriterWarning Warning;

            public INamespaceMapper Prefixes { get; private set; }

            public Uri BaseUri { get; private set; }

            public void Save(IGraph g, string filename)
            {
                throw new System.NotImplementedException();
            }

            public void Save(IGraph g, TextWriter output)
            {
                Prefixes = g.NamespaceMap;
                BaseUri = g.BaseUri;
            }
        }

        private class CompressingSerializerTestable : CompressingSerializer
        {
            private readonly IRdfWriter _writer;

            public CompressingSerializerTestable(IEntitySerializer entitySerializer, IRdfWriter writer, INamespaceManager mapper)
                : base(RdfSerialization.Turtle, entitySerializer, mapper)
            {
                _writer = writer;
            }

            protected override IRdfWriter CreateWriter()
            {
                return _writer;
            }
        }
    }
}
