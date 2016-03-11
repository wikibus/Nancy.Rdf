using System.Collections.Generic;
using System.IO;
using JsonLD.Entities;
using VDS.RDF;
using VDS.RDF.Parsing.Handlers;
using VDS.RDF.Writing.Formatting;

namespace Nancy.Rdf.Responses
{
    /// <summary>
    /// Serializer for NTriples format
    /// </summary>
    public class NTriplesSerializer : RdfSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NTriplesSerializer"/> class.
        /// </summary>
        public NTriplesSerializer(IEntitySerializer entitySerializer)
            : base(RdfSerialization.NTriples, entitySerializer)
        {
        }

        /// <summary>
        /// Writes the RDF is proper serialization.
        /// </summary>
        protected override void WriteRdf(StreamWriter writer, IEnumerable<Triple> triples)
        {
            IRdfHandler h = new WriteThroughHandler(new NTriples11Formatter(), writer);
            h.StartRdf();

            foreach (var triple in triples)
            {
                h.HandleTriple(triple);
            }

            h.EndRdf(true);
        }
    }
}
