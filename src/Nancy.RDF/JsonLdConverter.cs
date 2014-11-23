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

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdConverter"/> class.
        /// </summary>
        /// <param name="contextProvider">The @context provider.</param>
        public JsonLdConverter(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        /// <summary>
        /// Gets the serialized JSON-LD object.
        /// </summary>
        public JObject GetJson(object model)
        {
            var json = JsonConvert.SerializeObject(
                model,
                Formatting.None,
                new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        NullValueHandling = NullValueHandling.Ignore
                    });

            var jsObject = JObject.Parse(json);
            jsObject.AddFirst(new JProperty("@context", _contextProvider.GetContext(model.GetType())));
            return jsObject;
        }
    }
}