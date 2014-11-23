using Newtonsoft.Json.Linq;

namespace Nancy.RDF
{
    /// <summary>
    /// Contract for converting models to JSON-LD objects
    /// </summary>
    public interface IJsonLdConverter
    {
        /// <summary>
        /// Gets the serialized JSON-LD object.
        /// </summary>
        JObject GetJson(object model);
    }
}