using JsonLD.Entities;
using VDS.RDF;
using VDS.RDF.Writing;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Serializer for RDF/XML format
    /// </summary>
    public class RdfXmlSerializer : CompressingSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RdfXmlSerializer"/> class.
        /// </summary>
        public RdfXmlSerializer(IEntitySerializer entitySerializer, INamespaceMapper prefixMapper)
            : base(RdfSerialization.RdfXml, entitySerializer, prefixMapper)
        {
        }

        /// <summary>
        /// Creates the RDF writer.
        /// </summary>
        protected override IRdfWriter CreateWriter()
        {
            return new RdfXmlWriter();
        }
    }
}
