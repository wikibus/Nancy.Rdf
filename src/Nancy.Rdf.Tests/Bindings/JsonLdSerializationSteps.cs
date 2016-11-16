using System.IO;
using FakeItEasy;
using Nancy.Rdf.Contexts;
using Nancy.Rdf.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;

namespace Nancy.Rdf.Tests.Bindings
{
    [Binding, Scope(Tag = "JsonLd")]
    public class JsonLdSerializationSteps
    {
        private readonly ISerializer _serializer;
        private readonly SerializationContext _context;

        public JsonLdSerializationSteps(SerializationContext context)
        {
            _context = context;
            _serializer = new JsonLdSerializer(_context.Serializer, A.Dummy<IContextPathMapper>());
        }

        [When(@"model is serialized"), Scope(Tag = "JsonLd")]
        public void WhenModelIsSerializedTo()
        {
            var contentType = _context.AcceptHeader ?? RdfSerialization.JsonLd.MediaType;
            _serializer.Serialize(contentType, new object(), _context.OutputStream);

            _context.OutputStream.Seek(0, SeekOrigin.Begin);
            using (var streamReader = new StreamReader(_context.OutputStream))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    _context.Result = JToken.ReadFrom(jsonTextReader);
                }
            }
        }
    }
}
