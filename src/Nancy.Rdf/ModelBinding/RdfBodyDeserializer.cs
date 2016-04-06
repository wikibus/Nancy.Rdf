using System;
using System.IO;
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
        private readonly RdfSerialization _serialization;

        /// <summary>
        /// The serializer
        /// </summary>
        protected readonly IEntitySerializer Serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RdfBodyDeserializer"/> class.
        /// </summary>
        protected RdfBodyDeserializer(RdfSerialization serialization, IEntitySerializer serializer)
        {
            _serialization = serialization;
            Serializer = serializer;
        }

        /// <summary>
        /// Determines whether this instance can deserialize the specified content type.
        /// </summary>
        /// <returns>true for any of <see cref="RdfSerialization"/></returns>
        public bool CanDeserialize(string contentType, BindingContext context)
        {
            return new MediaRange(contentType).Matches(_serialization.MediaType);
        }

        /// <summary>
        /// Deserialize the request body to a model
        /// </summary>
        public abstract object Deserialize(string contentType, Stream body, BindingContext context);
    }
}
