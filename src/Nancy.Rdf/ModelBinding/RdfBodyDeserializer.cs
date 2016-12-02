using System.IO;
using System.Linq;
using JsonLD.Entities;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;

namespace Nancy.Rdf.ModelBinding
{
    /// <summary>
    /// Deserializes RDF bodies to POCO models
    /// </summary>
    public abstract class RdfBodyDeserializer : IBodyDeserializer
    {
        private readonly MediaRange[] serializations;

        /// <summary>
        /// Initializes a new instance of the <see cref="RdfBodyDeserializer"/> class.
        /// </summary>
        protected RdfBodyDeserializer(IEntitySerializer serializer, params RdfSerialization[] serializations)
        {
            this.serializations = serializations.Select(s => new MediaRange(s.MediaType)).ToArray();
            this.Serializer = serializer;
        }

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        protected IEntitySerializer Serializer { get; }

        /// <summary>
        /// Determines whether this instance can deserialize the specified content type.
        /// </summary>
        /// <returns>true for any of <see cref="RdfSerialization"/></returns>
        public bool CanDeserialize(MediaRange contentType, BindingContext context)
        {
            return this.serializations.Any(contentType.Matches);
        }

        /// <summary>
        /// Deserialize the request body to a model
        /// </summary>
        public abstract object Deserialize(MediaRange contentType, Stream body, BindingContext context);
    }
}
