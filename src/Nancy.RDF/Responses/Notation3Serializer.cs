using JsonLD.Entities;
using VDS.RDF;
using VDS.RDF.Writing;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Serializer for Notation3 format
    /// </summary>
    public class Notation3Serializer : CompressingSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notation3Serializer"/> class.
        /// </summary>
        public Notation3Serializer(IEntitySerializer entitySerializer, INamespaceMapper prefixMapper)
            : base(RdfSerialization.Notation3, entitySerializer, prefixMapper)
        {
        }

        /// <summary>
        /// Creates the RDF writer.
        /// </summary>
        protected override IRdfWriter CreateWriter()
        {
            return new Notation3Writer();
        }
    }
}
