using JsonLD.Entities;
using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Writing.Formatting;

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
        public RdfXmlSerializer(IEntitySerializer entitySerializer)
            : base(RdfSerialization.RdfXml, entitySerializer)
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
