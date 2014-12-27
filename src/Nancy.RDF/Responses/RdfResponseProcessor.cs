using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.Responses.Negotiation;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Response processor for RDF media types
    /// </summary>
    public abstract class RdfResponseProcessor : IResponseProcessor
    {
        private readonly RdfSerialization _serialization;
        private readonly RdfResponseOptions _options;
        private readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RdfResponseProcessor"/> class.
        /// </summary>
        /// <param name="serialization">The supported serialization.</param>
        /// <param name="serializers">The serializers.</param>
        /// <param name="options">Response processing options</param>
        protected RdfResponseProcessor(RdfSerialization serialization, IEnumerable<ISerializer> serializers, RdfResponseOptions options)
        {
            _serialization = serialization;
            _options = options;
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
            var match = new ProcessorMatch
            {
                ModelResult = MatchResult.DontCare
            };

            if (_serializer != null)
            {
                if (new MediaRange(_serialization.MediaType).Matches(requestedMediaRange))
                {
                    if (requestedMediaRange.IsWildcard == false)
                    {
                        match.RequestedContentTypeResult = MatchResult.ExactMatch;
                    }
                    else if (_options.FallbackSerialization == _serialization)
                    {
                        match.RequestedContentTypeResult = MatchResult.NonExactMatch;
                    }
                }
            }

            return match;
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
