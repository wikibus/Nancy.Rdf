using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.Responses.Negotiation;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Response processor for RDF media types (other than JSON-LD)
    /// </summary>
    public abstract class RdfResponseProcessor : IResponseProcessor
    {
        private readonly RdfSerialization _serialization;
        private readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RdfResponseProcessor"/> class.
        /// </summary>
        /// <param name="serialization">The supported serialization.</param>
        /// <param name="serializers">The serializers.</param>
        protected RdfResponseProcessor(RdfSerialization serialization, IEnumerable<ISerializer> serializers)
        {
            _serialization = serialization;
            _serializer = serializers.FirstOrDefault(s => s.CanSerialize(serialization.MediaType));
        }

        /// <summary>
        /// Gets the RDF extension mappings
        /// </summary>
        public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings
        {
            get
            {
                yield return Tuple.Create(_serialization.Extension, new MediaRange(_serialization.MediaType));
            }
        }

        /// <summary>
        /// Determines whether the given <paramref name="model"/> and be processed in <paramref name="requestedMediaRange"/>
        /// </summary>
        public ProcessorMatch CanProcess(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            var processorMatch = new ProcessorMatch
                {
                    ModelResult = MatchResult.DontCare
                };
            if (new MediaRange(_serialization.MediaType).Matches(requestedMediaRange) && !requestedMediaRange.IsWildcard)
            {
                processorMatch.RequestedContentTypeResult = MatchResult.ExactMatch;
            }

            return processorMatch;
        }

        /// <summary>
        /// Processes the model
        /// </summary>
        /// <returns>a response</returns>
        public Response Process(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            return new Response
                {
                    Contents = stream => _serializer.Serialize(_serialization.MediaType, model, stream),
                    StatusCode = HttpStatusCode.OK,
                    ContentType = _serialization.MediaType
                };
        }
    }
}
