using JsonLD.Entities;
using Nancy.RDF.Responses;
using TechTalk.SpecFlow;
using VDS.RDF;
using VDS.RDF.Writing.Formatting;

namespace Nancy.RDF.Tests.Bindings
{
    [Binding]
    public class RdfSerializationSteps
    {
        private readonly SerializationContext _context;
        private readonly ITripleFormatter _formatter;
        private readonly RdfSerializerTestable _serializer;

        public RdfSerializationSteps(SerializationContext context, IGraph graph)
        {
            _context = context;

            _formatter = new TestFormatter(graph);
            _serializer = new RdfSerializerTestable(_context.Serializer, _formatter);
        }

        [When(@"model is serialized"), Scope(Tag = "Rdf")]
        public void WhenModelIsSerialized()
        {
            _serializer.Serialize(RdfSerialization.Turtle.MediaType, new object(), _context.OutputStream);
        }

        private class RdfSerializerTestable : RdfSerializer
        {
            private readonly ITripleFormatter _formatter;

            public RdfSerializerTestable(IEntitySerializer entitySerializer, ITripleFormatter formatter)
                : base(RdfSerialization.Turtle, entitySerializer)
            {
                _formatter = formatter;
            }

            protected override ITripleFormatter CreateFormatter()
            {
                return _formatter;
            }
        }

        private class TestFormatter : ITripleFormatter
        {
            private readonly IGraph _graph;

            public TestFormatter(IGraph graph)
            {
                _graph = graph;
            }

            public string Format(Triple t)
            {
                _graph.Assert(t);

                return string.Empty;
            }
        }
    }
}
