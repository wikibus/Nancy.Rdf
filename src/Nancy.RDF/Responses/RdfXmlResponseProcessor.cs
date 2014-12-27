using System.Collections.Generic;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Response processor for RDF/XML
    /// </summary>
    public class RdfXmlResponseProcessor : RdfResponseProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RdfXmlResponseProcessor"/> class.
        /// </summary>
        /// <param name="serializers">The serializers.</param>
        /// <param name="options">Response processing options</param>
        public RdfXmlResponseProcessor(IEnumerable<ISerializer> serializers, RdfResponseOptions options)
            : base(RdfSerialization.RdfXml, serializers, options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RdfXmlResponseProcessor"/> class.
        /// </summary>
        /// <param name="serializers">The serializers.</param>
        public RdfXmlResponseProcessor(IEnumerable<ISerializer> serializers)
            : base(RdfSerialization.RdfXml, serializers, new RdfResponseOptions())
        {
        }
    }
}
