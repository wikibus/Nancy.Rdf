using System;
using System.Collections.Generic;
using System.IO;
using Nancy.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Serializer of JSON-LD
    /// </summary>
    public class JsonLdSerializer : ISerializer
    {
        private static readonly RdfSerialization JsonLdSerialization = RdfSerialization.JsonLd;

        private IContextProvider _contextProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdSerializer"/> class.
        /// </summary>
        /// <param name="contextProvider">The @context provider.</param>
        public JsonLdSerializer(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        /// <summary>
        /// Gets the list of extensions that the serializer can handle.
        /// </summary>
        /// <value>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of extensions if any are available, otherwise an empty enumerable.
        /// </value>
        public IEnumerable<string> Extensions
        {
            get { yield return JsonLdSerialization.Extension; }
        }

        /// <summary>
        /// Whether the serializer can serialize the content type
        /// </summary>
        /// <param name="contentType">Content type to serialize</param>
        /// <returns>
        /// True if supported, false otherwise
        /// </returns>
        public bool CanSerialize(string contentType)
        {
            return JsonLdSerialization.MediaType.Equals(contentType, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Serializes the specified content type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="model">The model.</param>
        /// <param name="outputStream">The output stream.</param>
        public void Serialize<TModel>(string contentType, TModel model, Stream outputStream)
        {
            using (var writer = new StreamWriter(new UnclosableStreamWrapper(outputStream)))
            {
                var json = JsonConvert.SerializeObject(
                    model,
                    Formatting.None,
                    new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });

                JObject jsObject = JObject.Parse(json);
                jsObject["@context"] = _contextProvider.GetContext<TModel>();

                writer.Write(jsObject);
            }
        }
    }
}
