using System;
using System.Diagnostics;
using System.Linq;
using Nancy.RDF.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Nancy.RDF
{
    /// <summary>
    /// Converts objects to JSON-LD
    /// </summary>
    public sealed class JsonLdConverter : IJsonLdConverter
    {
        private readonly IContextProvider _contextProvider;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdConverter"/> class.
        /// </summary>
        /// <param name="contextProvider">The @context provider.</param>
        /// <param name="contractResolver">Contract resolver, which defines JSON property names</param>
        public JsonLdConverter(IContextProvider contextProvider, IContractResolver contractResolver)
        {
            _contextProvider = contextProvider;
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdConverter"/> class.
        /// </summary>
        /// <param name="contextProvider">The @context provider.</param>
        public JsonLdConverter(IContextProvider contextProvider)
            : this(contextProvider, new JsonLdContractResolver())
        {
        }

        /// <summary>
        /// Gets the serialized JSON-LD object.
        /// </summary>
        public JObject GetJson(object model)
        {
            var json = JsonConvert.SerializeObject(model, Formatting.None, _jsonSerializerSettings);

            var jsObject = JObject.Parse(json);

            var types = GetTypes(model.GetType());
            if (types.Any())
            {
                jsObject.AddFirst(new JProperty("@types", types));
            }

            var context = _contextProvider.GetContext(model.GetType());
            if (context != null)
            {
                jsObject.AddFirst(new JProperty("@context", context));
            }

            Debug.WriteLine("Serialized model: {0}", jsObject);

            return jsObject;
        }

        private static JArray GetTypes(Type modelType)
        {
            var classes =
                from attr in modelType.GetCustomAttributes(typeof(ClassAttribute), false).OfType<ClassAttribute>()
                let classUri = attr.ClassUri
                select new JValue(classUri);

            return new JArray(classes.Cast<object>().ToArray());
        }
    }
}