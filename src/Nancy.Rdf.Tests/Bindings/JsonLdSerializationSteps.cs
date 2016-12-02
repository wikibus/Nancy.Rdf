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
        private readonly ISerializer serializer;
        private readonly SerializationContext context;

        public JsonLdSerializationSteps(SerializationContext context)
        {
            this.context = context;
            this.serializer = new JsonLdSerializer(this.context.Serializer, A.Dummy<IContextPathMapper>());
        }

        [When(@"model is serialized"), Scope(Tag = "JsonLd")]
        public void WhenModelIsSerializedTo()
        {
            var contentType = this.context.AcceptHeader ?? RdfSerialization.JsonLd.MediaType;
            this.serializer.Serialize(contentType, new object(), this.context.OutputStream);

            this.context.OutputStream.Seek(0, SeekOrigin.Begin);
            using (var streamReader = new StreamReader(this.context.OutputStream))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    this.context.Result = JToken.ReadFrom(jsonTextReader);
                }
            }
        }
    }
}
