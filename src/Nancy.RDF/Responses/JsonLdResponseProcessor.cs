using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.Responses.Negotiation;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Response processor for JSON-LD
    /// </summary>
    public class JsonLdResponseProcessor : IResponseProcessor
    {
        private readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdResponseProcessor"/> class.
        /// </summary>
        public JsonLdResponseProcessor(IEnumerable<ISerializer> serializers)
        {
            _serializer = serializers.FirstOrDefault(s => s.CanSerialize(RdfSerialization.JsonLd.MediaType));
        }

        /// <summary>
        /// Gets the JSON-LD extension mappings
        /// </summary>
        public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings
        {
            get
            {
                yield return Tuple.Create(RdfSerialization.JsonLd.Extension, new MediaRange(RdfSerialization.JsonLd.MediaType));
            }
        }

        /// <summary>
        /// Determines whether the given <paramref name="model"/> and be processed in <paramref name="requestedMediaRange"/>
        /// </summary>
        public ProcessorMatch CanProcess(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            return new ProcessorMatch
                {
                    ModelResult = MatchResult.DontCare,
                    RequestedContentTypeResult = MatchResult.ExactMatch
                };
        }

        /// <summary>
        /// Processes the model
        /// </summary>
        /// <returns>a response</returns>
        public Response Process(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            return new Response
                {
                    Contents = stream => _serializer.Serialize(requestedMediaRange.ToString(), model, stream),
                    ContentType = RdfSerialization.JsonLd.MediaType,
                    StatusCode = HttpStatusCode.OK
                };
        }
    }
}
