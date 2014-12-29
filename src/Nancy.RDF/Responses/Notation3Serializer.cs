using System;
using System.Collections.Generic;
using System.IO;
using JsonLD.Entities;
using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Writing.Formatting;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Serializer for Notation3 format
    /// </summary>
    public class Notation3Serializer : RdfSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notation3Serializer"/> class.
        /// </summary>
        public Notation3Serializer(IEntitySerializer entitySerializer)
            : base(RdfSerialization.Notation3, entitySerializer)
        {
        }

        /// <summary>
        /// Creates the triple formatter for Notation3.
        /// </summary>
        protected override ITripleFormatter CreateFormatter()
        {
            throw new NotImplementedException("Not used for Notation3");
        }

        /// <summary>
        /// Writes the RDF as compressed Turtle
        /// </summary>
        protected override void WriteRdf(StreamWriter writer, IEnumerable<Triple> triples)
        {
            var graph = new Graph();
            graph.Assert(triples);
            graph.SaveToStream(writer, new Notation3Writer());
        }
    }
}
