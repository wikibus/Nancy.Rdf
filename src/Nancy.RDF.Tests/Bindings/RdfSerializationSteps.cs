using System.IO;
using JsonLD.Entities;
using Nancy.RDF.Responses;
using TechTalk.SpecFlow;
using VDS.RDF;

namespace Nancy.RDF.Tests.Bindings
{
    [Binding]
    public class RdfSerializationSteps
    {
        private readonly SerializationContext _context;
        private readonly RdfSerializerTestable _serializer;

        public RdfSerializationSteps(SerializationContext context, IGraph graph)
        {
            _context = context;

            _serializer = new RdfSerializerTestable(_context.Serializer, new TestWriter(graph));
        }

        [When(@"model is serialized"), Scope(Tag = "Rdf")]
        public void WhenModelIsSerialized()
        {
            _serializer.Serialize(RdfSerialization.Turtle.MediaType, new object(), _context.OutputStream);
        }

        private class RdfSerializerTestable : CompressingSerializer
        {
            private readonly IRdfWriter _writer;

            public RdfSerializerTestable(IEntitySerializer entitySerializer, IRdfWriter writer)
                : base(RdfSerialization.Turtle, entitySerializer)
            {
                _writer = writer;
            }

            protected override IRdfWriter CreateWriter()
            {
                return _writer;
            }
        }

        private class TestWriter : IRdfWriter
        {
            private readonly IGraph _graph;

            public TestWriter(IGraph graph)
            {
                _graph = graph;
            }

            public event RdfWriterWarning Warning;

            public void Save(IGraph g, string filename)
            {
                _graph.Assert(g.Triples);
            }

            public void Save(IGraph g, TextWriter output)
            {
                _graph.Assert(g.Triples);
            }
        }
    }
}
