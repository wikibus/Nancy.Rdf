using JsonLD.Entities;
using VDS.RDF.Parsing;

namespace Nancy.Rdf.ModelBinding
{
    /// <summary>
    /// Deserializes RDF/XML request body to model
    /// </summary>
    public class RdfXmlBodyDeserializer : NonJsonLdRdfBodyDeserializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RdfXmlBodyDeserializer"/> class.
        /// </summary>
        public RdfXmlBodyDeserializer(IEntitySerializer serializer)
            : base(RdfSerialization.RdfXml, serializer, new RdfXmlParser())
        {
        }
    }
}
