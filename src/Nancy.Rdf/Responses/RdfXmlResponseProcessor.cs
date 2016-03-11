using System.Collections.Generic;

namespace Nancy.Rdf.Responses
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
        public RdfXmlResponseProcessor(IEnumerable<ISerializer> serializers)
            : base(RdfSerialization.RdfXml, serializers)
        {
        }
    }
}
