using System.IO;
using Nancy.RDF.Responses;
using TechTalk.SpecFlow;
using VDS.RDF;

namespace Nancy.RDF.Tests.Bindings
{
    [Binding, Scope(Tag = "Turtle")]
    public class TurtleSerializationSteps
    {
        private readonly ISerializer _serializer;
        private readonly SerializationContext _context;
        private readonly IGraph _serialized;

        public TurtleSerializationSteps(SerializationContext context, IGraph graph)
        {
            _context = context;
            _serialized = graph;
            _serializer = new TurtleSerializer(new JsonLdSerializer(context.ContextProvider));
        }

        [When(@"model is serialized")]
        public void WhenModelIsSerializedTo()
        {
            _serializer.Serialize(RdfSerialization.JsonLd.MediaType, ScenarioContext.Current["model"], _context.OutputStream);

            _context.OutputStream.Seek(0, SeekOrigin.Begin);
            using (var streamReader = new StreamReader(_context.OutputStream))
            {
                _serialized.LoadFromString(streamReader.ReadToEnd());
            }
        }
    }
}
