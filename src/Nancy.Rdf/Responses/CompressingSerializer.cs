using System.Collections.Generic;
using System.IO;
using JsonLD.Entities;
using VDS.RDF;

namespace Nancy.Rdf.Responses
{
    /// <summary>
    /// An RDF serializer, which applies namespace compression
    /// </summary>
    public abstract class CompressingSerializer : RdfSerializer
    {
        private readonly INamespaceManager namespaces;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompressingSerializer" /> class.
        /// </summary>
        /// <param name="rdfSerialization">The RDF serialization.</param>
        /// <param name="entitySerializer">The entity serializer.</param>
        /// <param name="namespaces">The namespace mapper.</param>
        protected CompressingSerializer(RdfSerialization rdfSerialization, IEntitySerializer entitySerializer, INamespaceManager namespaces)
            : base(rdfSerialization, entitySerializer)
        {
            this.namespaces = namespaces;
        }

        /// <summary>
        /// Writes the RDF as compressed Turtle
        /// </summary>
        protected override void WriteRdf(StreamWriter writer, IEnumerable<Triple> triples)
        {
            var graph = new Graph(true) { BaseUri = this.namespaces.BaseUri };
            foreach (var ns in this.namespaces)
            {
                graph.NamespaceMap.AddNamespace(ns.Prefix, ns.Namespace);
            }

            graph.Assert(triples);
            graph.SaveToStream(writer, this.CreateWriter());
        }

        /// <summary>
        /// Creates the RDF writer.
        /// </summary>
        protected abstract IRdfWriter CreateWriter();
    }
}
