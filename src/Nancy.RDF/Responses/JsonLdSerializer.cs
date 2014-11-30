using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using JsonLD.Core;
using JsonLD.Entities;
using Nancy.IO;
using Nancy.Responses.Negotiation;
using Newtonsoft.Json.Linq;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Serializer of JSON-LD
    /// </summary>
    public class JsonLdSerializer : ISerializer
    {
        private static readonly RdfSerialization JsonLdSerialization = RdfSerialization.JsonLd;
        private readonly IEntitySerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdSerializer"/> class.
        /// </summary>
        public JsonLdSerializer(IEntitySerializer serializer)
        {
            _serializer = serializer;
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
                JToken serialized = _serializer.Serialize(model);

                var mediaRange = new MediaRange(contentType);

                if (mediaRange.Parameters["profile"] == JsonLdProfiles.Expanded)
                {
                    serialized = JsonLdProcessor.Expand(serialized);
                }

                Debug.WriteLine("Serialized model: {0}", new object[] { serialized });

                writer.Write(serialized);
            }
        }
    }
}
