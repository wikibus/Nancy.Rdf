using JsonLD.Entities;
using VDS.RDF.Parsing;

namespace Nancy.Rdf.ModelBinding
{
    /// <summary>
    /// Deserializes N3 request body to model
    /// </summary>
    public class Notation3BodyDeserializer : NonJsonLdRdfBodyDeserializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notation3BodyDeserializer"/> class.
        /// </summary>
        public Notation3BodyDeserializer(IEntitySerializer serializer)
            : base(RdfSerialization.Notation3, serializer, new Notation3Parser())
        {
        }
    }
}
