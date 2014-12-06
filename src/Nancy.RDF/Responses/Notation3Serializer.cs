using JsonLD.Entities;
using VDS.RDF.Writing.Formatting;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Serializer for Notation3 format
    /// </summary>
    public class Notation3Serializer : RdfSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notation3Serializer"/> class.
        /// </summary>
        public Notation3Serializer(IEntitySerializer entitySerializer)
            : base(RdfSerialization.NTriples, entitySerializer)
        {
        }

        /// <summary>
        /// Creates the triple formatter for Notation3.
        /// </summary>
        protected override ITripleFormatter CreateFormatter()
        {
            return new Notation3Formatter();
        }
    }
}
