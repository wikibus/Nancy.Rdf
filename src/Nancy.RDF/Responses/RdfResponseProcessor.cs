using System;
using System.Collections.Generic;
using Nancy.Responses.Negotiation;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Response processor for RDF media types (except JSON-LD)
    /// </summary>
    public class RdfResponseProcessor : IResponseProcessor
    {
        /// <summary>
        /// Gets the RDF extension mappings
        /// </summary>
        public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings
        {
            get
            {
                yield return Tuple.Create("ttl", new MediaRange(RdfSerialization.Turtle.MediaType));
                yield return Tuple.Create("rdf", new MediaRange(RdfSerialization.RdfXml.MediaType));
                yield return Tuple.Create("n3", new MediaRange(RdfSerialization.Notation3.MediaType));
                yield return Tuple.Create("nt", new MediaRange(RdfSerialization.NTriples.MediaType));
            }
        }

        /// <summary>
        /// Determines whether the given <paramref name="model"/> and be processed in <paramref name="requestedMediaRange"/>
        /// </summary>
        public ProcessorMatch CanProcess(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            return new ProcessorMatch();
        }

        /// <summary>
        /// Processes the model
        /// </summary>
        /// <returns>a response</returns>
        public Response Process(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            return null;
        }
 }
}
