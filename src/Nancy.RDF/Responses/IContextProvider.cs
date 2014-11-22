using Newtonsoft.Json.Linq;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Contract for classes, which provide JSON-LD @context for given types
    /// </summary>
    public interface IContextProvider
    {
        /// <summary>
        /// Gets the JSON-LD @context for a given serialized type.
        /// </summary>
        /// <typeparam name="T">type of entity</typeparam>
        JToken GetContext<T>();
    }
}
