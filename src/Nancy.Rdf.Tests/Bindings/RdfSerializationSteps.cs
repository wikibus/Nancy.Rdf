using System.IO;
using FakeItEasy;
using JsonLD.Entities;
using Nancy.Rdf.Responses;
using TechTalk.SpecFlow;
using VDS.RDF;

namespace Nancy.Rdf.Tests.Bindings
{
    [Binding]
    public class RdfSerializationSteps
    {
        private readonly SerializationContext context;
        private readonly RdfSerializerTestable serializer;

        public RdfSerializationSteps(SerializationContext context, IGraph graph)
        {
            this.context = context;

            this.serializer = new RdfSerializerTestable(this.context.Serializer, new TestWriter(graph), A.Dummy<INamespaceManager>());
        }

        [When(@"model is serialized"), Scope(Tag = "Rdf")]
        public void WhenModelIsSerialized()
        {
            this.serializer.Serialize(RdfSerialization.Turtle.MediaType, new object(), this.context.OutputStream);
        }

        private class RdfSerializerTestable : CompressingSerializer
        {
            private readonly IRdfWriter writer;

            public RdfSerializerTestable(IEntitySerializer entitySerializer, IRdfWriter writer, INamespaceManager mapper)
                : base(RdfSerialization.Turtle, entitySerializer, mapper)
            {
                this.writer = writer;
            }

            protected override IRdfWriter CreateWriter()
            {
                return this.writer;
            }
        }

        private class TestWriter : IRdfWriter
        {
            private readonly IGraph graph;

            public TestWriter(IGraph graph)
            {
                this.graph = graph;
            }

            public event RdfWriterWarning Warning;

            public void Save(IGraph g, string filename)
            {
                this.graph.Assert(g.Triples);
            }

            public void Save(IGraph g, TextWriter output)
            {
                this.graph.Assert(g.Triples);
            }
        }
    }
}
