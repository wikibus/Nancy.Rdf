using System;
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
        private INamespaceMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var entitySerialier = A.Fake<IEntitySerializer>();
            _writer = new WriterSpy();
            _mapper = new NamespaceMapper(true);
            _serializer = new CompressingSerializerTestable(entitySerialier, _writer, _mapper);
        }

        [Test]
        public void Serialized_graph_should_use_namespaces_if_given()
        {
            // given
            _mapper.AddNamespace("ex", new Uri("http://example.com"));
            _mapper.AddNamespace("lol", new Uri("http://lol.com"));

            // when
            _serializer.Serialize(RdfSerialization.Turtle.MediaType, new object(), new MemoryStream());

            // then
            Assert.That(_writer.Prefixes.Prefixes, Has.Count.EqualTo(2));
            Assert.That(_writer.Prefixes.HasNamespace("ex"));
            Assert.That(_writer.Prefixes.HasNamespace("lol"));
        }

        private class WriterSpy : IRdfWriter
        {
            public event RdfWriterWarning Warning;

            public INamespaceMapper Prefixes { get; private set; }

            public void Save(IGraph g, string filename)
            {
                throw new System.NotImplementedException();
            }

            public void Save(IGraph g, TextWriter output)
            {
                Prefixes = g.NamespaceMap;
            }
        }

        private class CompressingSerializerTestable : CompressingSerializer
        {
            private readonly IRdfWriter _writer;

            public CompressingSerializerTestable(IEntitySerializer entitySerializer, IRdfWriter writer, INamespaceMapper mapper)
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
