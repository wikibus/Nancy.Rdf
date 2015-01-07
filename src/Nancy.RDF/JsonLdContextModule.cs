using System;
using JsonLD.Entities;
using Nancy.Responses;
using Newtonsoft.Json.Linq;

namespace Nancy.RDF
{
    /// <summary>
    /// Module, which serves JSON-LD contexts
    /// </summary>
    public abstract class JsonLdContextModule : NancyModule
    {
        private readonly ContextResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdContextModule"/> class.
        /// </summary>
        /// <param name="provider">custom context provider</param>
        protected JsonLdContextModule(IContextProvider provider)
        {
            _resolver = new ContextResolver(provider);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdContextModule"/> class.
        /// </summary>
        /// <param name="modulePath">A base path for contexts.</param>
        /// <param name="provider">custom context provider</param>
        protected JsonLdContextModule(string modulePath, IContextProvider provider) : base(modulePath)
        {
            _resolver = new ContextResolver(provider);
        }

        /// <summary>
        /// Serves a static JSON-LD context
        /// </summary>
        protected Func<object, object> ServeContext(string context)
        {
            return ServeContext(JToken.Parse(context));
        }

        /// <summary>
        /// Serves a static JSON-LD context
        /// </summary>
        protected Func<object, object> ServeContext(JToken context)
        {
            return request =>
            {
                var response = new TextResponse(context.ToString(), RdfSerialization.JsonLd.MediaType);
                return Negotiate.WithAllowedMediaRange(RdfSerialization.JsonLd.MediaType)
                                .WithMediaRangeResponse(RdfSerialization.JsonLd.MediaType, response);
            };
        }

        /// <summary>
        /// Serves the context of given type.
        /// </summary>
        /// <typeparam name="T">JSON-LD model type</typeparam>
        protected Func<object, object> ServeContextOf<T>()
        {
            return ServeContext(_resolver.GetContext(typeof(T)));
        }
    }
}
