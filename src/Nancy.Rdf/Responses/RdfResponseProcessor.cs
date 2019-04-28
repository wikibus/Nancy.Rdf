using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.Responses.Negotiation;
using NullGuard;

namespace Nancy.Rdf.Responses
{
    /// <summary>
    /// Response processor for RDF media types
    /// </summary>
    public abstract class RdfResponseProcessor : IResponseProcessor
    {
        private readonly RdfSerialization serialization;
        private readonly ISerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RdfResponseProcessor"/> class.
        /// </summary>
        /// <param name="serialization">The supported serialization.</param>
        /// <param name="serializers">The serializers.</param>
        protected RdfResponseProcessor(RdfSerialization serialization, IEnumerable<ISerializer> serializers)
        {
            this.serialization = serialization;
            this.serializer = serializers.Cast<IRdfSerializer>().FirstOrDefault(s => s.CanSerialize(serialization.MediaType));
        }

        /// <summary>
        /// Gets the RDF extension mappings
        /// </summary>
        public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings
        {
            get
            {
                yield return Tuple.Create(this.serialization.Extension, new MediaRange(this.serialization.MediaType));
            }
        }

        /// <summary>
        /// Determines whether the given <paramref name="model"/> and be processed in <paramref name="requestedMediaRange"/>
        /// </summary>
        public ProcessorMatch CanProcess(MediaRange requestedMediaRange, [AllowNull] dynamic model, NancyContext context)
        {
            var match = new ProcessorMatch
            {
                ModelResult = model == null ? MatchResult.NoMatch : MatchResult.DontCare
            };

            if (this.serializer != null)
            {
                if (new MediaRange(this.serialization.MediaType).Matches(requestedMediaRange))
                {
                    if (requestedMediaRange.IsWildcard == false)
                    {
                        match.RequestedContentTypeResult = MatchResult.ExactMatch;
                    }
                    else if (GetFallbackSerialization(context) == this.serialization)
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
            var siteBaseUri = new UriBuilder(context.Request.Url.SiteBase);

            if (context.Request.Headers["X-Forwarded-Proto"].Any(v => v == "https"))
            {
                siteBaseUri.Scheme = "HTTPS";
                siteBaseUri.Port = 443;
            }

            string siteBase;
            var usesHttpsDefaultPort = siteBaseUri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) &&
                                       siteBaseUri.Port == 443;
            var usesHttpDefaultPort = siteBaseUri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) &&
                                      siteBaseUri.Port == 80;
            if (usesHttpDefaultPort || usesHttpsDefaultPort)
            {
                siteBase = siteBaseUri.Uri.GetComponents(
                    UriComponents.AbsoluteUri & ~UriComponents.Port,
                    UriFormat.UriEscaped);
            }
            else
            {
                siteBase = siteBaseUri.ToString();
            }

            return new Response
                {
                    Contents = stream =>
                    {
                        var wrappedModel = new WrappedModel(model, siteBase);
                        this.serializer.Serialize(requestedMediaRange, wrappedModel, stream);
                    },
                    StatusCode = HttpStatusCode.OK,
                    ContentType = this.serialization.MediaType
                };
        }

        private static RdfSerialization? GetFallbackSerialization(NancyContext context)
        {
            if (context.Items.ContainsKey(RdfResponses.FallbackSerializationKey))
            {
                return (RdfSerialization)context.Items[RdfResponses.FallbackSerializationKey];
            }

            return null;
        }
    }
}
