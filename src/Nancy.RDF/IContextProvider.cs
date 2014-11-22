using System;
using Newtonsoft.Json.Linq;

namespace Nancy.RDF
{
    /// <summary>
    /// Contract for classes, which provide JSON-LD @context for given types
    /// </summary>
    public interface IContextProvider
    {
        /// <summary>
        /// Gets the JSON-LD @context for a given serialized type.
        /// </summary>
        /// <param name="modelType">Type of the entity.</param>
        JToken GetContext(Type modelType);
    }
}
