using VDS.RDF.Writing.Formatting;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Serializer for Turtle RDF format
    /// </summary>
    public class TurtleSerializer : RdfSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TurtleSerializer"/> class.
        /// </summary>
        public TurtleSerializer(JsonLdSerializer jsonLdSerializer) 
            : base(RdfSerialization.Turtle, jsonLdSerializer)
        {
        }

        /// <summary>
        /// Creates the triple formatter for Turtle.
        /// </summary>
        protected override ITripleFormatter CreateFormatter()
        {
            return new TurtleW3CFormatter();
        }
    }
}
