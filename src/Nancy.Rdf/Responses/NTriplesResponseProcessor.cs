using System.Collections.Generic;

namespace Nancy.Rdf.Responses
{
    /// <summary>
    /// Response processor for Turtle
    /// </summary>
    public class NTriplesResponseProcessor : RdfResponseProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NTriplesResponseProcessor"/> class.
        /// </summary>
        /// <param name="serializers">The serializers.</param>
        public NTriplesResponseProcessor(IEnumerable<ISerializer> serializers)
            : base(RdfSerialization.NTriples, serializers)
        {
        }
    }
}
