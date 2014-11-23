using System;
using System.Diagnostics;
using System.IO;
using Nancy.RDF.Responses;
using TechTalk.SpecFlow;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace Nancy.RDF.Tests.Bindings
{
    [Binding]
    public class RdfSerializationSteps
    {
        private readonly SerializationContext _context;
        private readonly IGraph _serialized;

        public RdfSerializationSteps(SerializationContext context, IGraph graph)
        {
            _context = context;
            _serialized = graph;
        }

        [When(@"model is serialized"), Scope(Tag = "Turtle")]
        public void WhenModelIsSerializedToTurtle()
        {
            var serializer = new TurtleSerializer(_context.ContextProvider);
            var serialized = SerializeModel(serializer, RdfSerialization.Turtle);
            _serialized.LoadFromString(serialized, new TurtleParser());
        }

        [When(@"model is serialized"), Scope(Tag = "RdfXml")]
        public void WhenModelIsSerializedToRdfXml()
        {
            var serializer = new RdfXmlSerializer(_context.ContextProvider);
            var serialized = SerializeModel(serializer, RdfSerialization.RdfXml);
            _serialized.LoadFromString(serialized, new RdfXmlParser());
        }

        [When(@"model is serialized"), Scope(Tag = "NTriples")]
        public void WhenModelIsSerializedToNTriples()
        {
            var serializer = new NTriplesSerializer(_context.ContextProvider);
            var serialized = SerializeModel(serializer, RdfSerialization.RdfXml);
            _serialized.LoadFromString(serialized, new NTriplesParser(NTriplesSyntax.Rdf11));
        }

        private string SerializeModel(ISerializer serializer, RdfSerialization serialization)
        {
            serializer.Serialize(serialization.MediaType, ScenarioContext.Current["model"], _context.OutputStream);

            _context.OutputStream.Seek(0, SeekOrigin.Begin);
            using (var streamReader = new StreamReader(_context.OutputStream))
            {
                var serializedGraph = streamReader.ReadToEnd();

                Debug.WriteLine("Deserialized graph contents:{0}{1}", Environment.NewLine, serializedGraph);

                return serializedGraph;
            }
        }
    }
}
