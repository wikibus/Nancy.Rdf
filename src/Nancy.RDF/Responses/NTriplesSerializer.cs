using VDS.RDF.Writing.Formatting;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Serializer for NTriples format
    /// </summary>
    public class NTriplesSerializer : RdfSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NTriplesSerializer"/> class.
        /// </summary>
        public NTriplesSerializer(IContextProvider contextProvider)
            : base(RdfSerialization.NTriples, contextProvider)
        {
        }

        /// <summary>
        /// Creates the triple formatter for NTriples.
        /// </summary>
        protected override ITripleFormatter CreateFormatter()
        {
            return new NTriples11Formatter();
        }
    }
}
