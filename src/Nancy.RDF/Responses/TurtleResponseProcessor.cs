using System.Collections.Generic;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Response processor for Turtle
    /// </summary>
    public class TurtleResponseProcessor : RdfResponseProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TurtleResponseProcessor"/> class.
        /// </summary>
        /// <param name="serializers">The serializers.</param>
        /// <param name="options">Response processing options</param>
        public TurtleResponseProcessor(IEnumerable<ISerializer> serializers, RdfResponseOptions options)
            : base(RdfSerialization.Turtle, serializers, options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TurtleResponseProcessor"/> class.
        /// </summary>
        /// <param name="serializers">The serializers.</param>
        public TurtleResponseProcessor(IEnumerable<ISerializer> serializers)
            : base(RdfSerialization.Turtle, serializers, new RdfResponseOptions())
        {
        }
    }
}
