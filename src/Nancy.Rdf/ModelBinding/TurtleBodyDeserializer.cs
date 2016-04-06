using JsonLD.Entities;
using VDS.RDF.Parsing;

namespace Nancy.Rdf.ModelBinding
{
    /// <summary>
    /// Deserializes Turtle request body to model
    /// </summary>
    public class TurtleBodyDeserializer : NonJsonLdRdfBodyDeserializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TurtleBodyDeserializer"/> class.
        /// </summary>
        public TurtleBodyDeserializer(IEntitySerializer serializer)
            : base(RdfSerialization.Turtle, serializer, new TurtleParser())
        {
        }
    }
}
