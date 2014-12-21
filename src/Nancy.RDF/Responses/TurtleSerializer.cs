using System;
using System.Collections.Generic;
using System.IO;
using JsonLD.Entities;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
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
        public TurtleSerializer(IEntitySerializer entitySerializer)
            : base(RdfSerialization.Turtle, entitySerializer)
        {
        }

        /// <summary>
        /// Creates the triple formatter for Turtle.
        /// </summary>
        protected override ITripleFormatter CreateFormatter()
        {
            throw new NotImplementedException("Not used for Turtle");
        }

        /// <summary>
        /// Writes the RDF as compressed Turtle
        /// </summary>
        protected override void WriteRdf(StreamWriter writer, IEnumerable<Triple> triples)
        {
            var graph = new Graph();
            graph.Assert(triples);
            graph.SaveToStream(writer, new CompressingTurtleWriter(TurtleSyntax.W3C));
        }
    }
}
