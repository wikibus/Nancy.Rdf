using System;
using JsonLD.Entities;
using Nancy.Responses;
using Newtonsoft.Json.Linq;

namespace Nancy.Rdf.Contexts
{
    /// <summary>
    /// Module, which serves JSON-LD contexts
    /// </summary>
    public class JsonLdContextModule : NancyModule
    {
        private readonly ContextResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdContextModule"/> class.
        /// </summary>
        /// <param name="pathProvider">@context path provider</param>
        /// <param name="provider">custom @context provider</param>
        public JsonLdContextModule(IContextPathMapper pathProvider, IContextProvider provider)
            : base(pathProvider.BasePath)
        {
            this.resolver = new ContextResolver(provider);

            foreach (var path in pathProvider.Contexts)
            {
                this.Get(path.Path, this.ServeContextOf(path.ModelType));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdContextModule"/> class.
        /// </summary>
        public JsonLdContextModule()
        {
        }

        private Func<object, object> ServeContextOf(Type modelType)
        {
            return request =>
            {
                var context = new JObject
                {
                    { JsonLdKeywords.Context, this.resolver.GetContext(modelType) }
                };
                var response = new TextResponse(context.ToString(), RdfSerialization.JsonLd.MediaType);
                return this.Negotiate.WithAllowedMediaRange(RdfSerialization.JsonLd.MediaType)
                                .WithMediaRangeResponse(RdfSerialization.JsonLd.MediaType, response);
            };
        }
    }
}
