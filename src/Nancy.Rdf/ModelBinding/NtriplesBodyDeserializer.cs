using System.IO;
using JsonLD.Entities;
using VDS.RDF.Parsing;

namespace Nancy.Rdf.ModelBinding
{
    /// <summary>
    /// Deserializes N-Triples request body to model
    /// </summary>
    public class NtriplesBodyDeserializer : NonJsonLdRdfBodyDeserializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NtriplesBodyDeserializer"/> class.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        public NtriplesBodyDeserializer(IEntitySerializer serializer)
            : base(RdfSerialization.NTriples, serializer, new NTriplesParser())
        {
        }

        /// <summary>
        /// Reads the <paramref name="body" /> into NQuads.
        /// </summary>
        protected override string GetNquads(Stream body)
        {
            body.Position = 0;
            using (var bodyReader = new StreamReader(body))
            {
                return bodyReader.ReadToEnd();
            }
        }
    }
}
