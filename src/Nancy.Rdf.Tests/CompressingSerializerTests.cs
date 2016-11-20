using System;
using System.Collections.Generic;
using System.IO;
using FakeItEasy;
using JsonLD.Entities;
using Nancy.Rdf.Responses;
using NUnit.Framework;
using VDS.RDF;

namespace Nancy.Rdf.Tests
{
    [TestFixture]
    public class CompressingSerializerTests
    {
        private CompressingSerializer serializer;
        private WriterSpy writer;
        private INamespaceManager mapper;

        [SetUp]
        public void Setup()
        {
            var entitySerialier = A.Fake<IEntitySerializer>();
            this.writer = new WriterSpy();
            this.mapper = A.Fake<INamespaceManager>();
            this.serializer = new CompressingSerializerTestable(entitySerialier, this.writer, this.mapper);
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
            A.CallTo(() => this.mapper.GetEnumerator()).Returns(prefixes.GetEnumerator());

            // when
            this.serializer.Serialize(RdfSerialization.Turtle.MediaType, new object(), new MemoryStream());

            // then
            Assert.That(this.writer.Prefixes.Prefixes, Has.Count.EqualTo(2));
            Assert.That(this.writer.Prefixes.HasNamespace("ex"));
            Assert.That(this.writer.Prefixes.HasNamespace("lol"));
        }

        [Test]
        public void Serialized_graph_should_use_base_uri_if_given()
        {
            // given
            var baseUri = new Uri("http://example.org/base");
            A.CallTo(() => this.mapper.BaseUri).Returns(baseUri);

            // when
            this.serializer.Serialize(RdfSerialization.Turtle.MediaType, new object(), new MemoryStream());

            // then
            Assert.That(this.writer.BaseUri, Is.EqualTo(baseUri));
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
                this.Prefixes = g.NamespaceMap;
                this.BaseUri = g.BaseUri;
            }
        }

        private class CompressingSerializerTestable : CompressingSerializer
        {
            private readonly IRdfWriter writer;

            public CompressingSerializerTestable(IEntitySerializer entitySerializer, IRdfWriter writer, INamespaceManager mapper)
                : base(RdfSerialization.Turtle, entitySerializer, mapper)
            {
                this.writer = writer;
            }

            protected override IRdfWriter CreateWriter()
            {
                return this.writer;
            }
        }
    }
}
