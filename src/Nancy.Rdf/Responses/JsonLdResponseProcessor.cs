using System.Collections.Generic;

namespace Nancy.Rdf.Responses
{
    /// <summary>
    /// Response processor for JSON-LD
    /// </summary>
    public class JsonLdResponseProcessor : RdfResponseProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdResponseProcessor"/> class.
        /// </summary>
        public JsonLdResponseProcessor(IEnumerable<ISerializer> serializers)
            : base(RdfSerialization.JsonLd, serializers)
        {
        }
    }
}
