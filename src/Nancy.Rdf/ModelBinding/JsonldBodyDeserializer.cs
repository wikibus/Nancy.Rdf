using System.IO;
using System.Reflection;
using JsonLD.Entities;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nancy.Rdf.ModelBinding
{
    /// <summary>
    /// Deserializes JSON-LD request body to model
    /// </summary>
    public class JsonldBodyDeserializer : RdfBodyDeserializer
    {
        private static readonly MethodInfo DeserializeJsonLdMethod = typeof(IEntitySerializer).GetMethod("Deserialize", new[] { typeof(JToken) });

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonldBodyDeserializer"/> class.
        /// </summary>
        public JsonldBodyDeserializer(IEntitySerializer serializer)
            : base(RdfSerialization.JsonLd, serializer)
        {
        }

        /// <summary>
        /// Deserialize the request body to a model
        /// </summary>
        public override object Deserialize(MediaRange contentType, Stream body, BindingContext context)
        {
            body.Position = 0;
            using (var bodyReader = new StreamReader(body))
            {
                var deserialize = DeserializeJsonLdMethod.MakeGenericMethod(context.DestinationType);

                return deserialize.Invoke(Serializer, new object[] { JToken.ReadFrom(new JsonTextReader(bodyReader)) });
            }
        }
    }
}
