using JsonLD.Entities;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Serializer for Turtle RDF format
    /// </summary>
    public class TurtleSerializer : CompressingSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TurtleSerializer"/> class.
        /// </summary>
        public TurtleSerializer(IEntitySerializer entitySerializer)
            : base(RdfSerialization.Turtle, entitySerializer)
        {
        }

        /// <summary>
        /// Creates the RDF writer.
        /// </summary>
        protected override IRdfWriter CreateWriter()
        {
            return new CompressingTurtleWriter(TurtleSyntax.W3C);
        }
    }
}
