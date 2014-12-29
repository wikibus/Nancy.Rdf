using System.Collections.Generic;
using System.IO;
using JsonLD.Entities;
using VDS.RDF;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// An RDF serializer, which applies namespace compression
    /// </summary>
    public abstract class CompressingSerializer : RdfSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompressingSerializer"/> class.
        /// </summary>
        /// <param name="rdfSerialization">The RDF serialization.</param>
        /// <param name="entitySerializer">The entity serializer.</param>
        protected CompressingSerializer(RdfSerialization rdfSerialization, IEntitySerializer entitySerializer)
            : base(rdfSerialization, entitySerializer)
        {
        }

        /// <summary>
        /// Writes the RDF as compressed Turtle
        /// </summary>
        protected override void WriteRdf(StreamWriter writer, IEnumerable<Triple> triples)
        {
            var graph = new Graph();
            graph.Assert(triples);
            graph.SaveToStream(writer, CreateWriter());
        }

        /// <summary>
        /// Creates the RDF writer.
        /// </summary>
        protected abstract IRdfWriter CreateWriter();
    }
}
