using System;
using Newtonsoft.Json.Serialization;

namespace Nancy.RDF
{
    /// <summary>
    /// Camel-case contract resolver with overrides for JSON-LD keywords
    /// </summary>
    public class JsonLdContractResolver : CamelCasePropertyNamesContractResolver
    {
        /// <summary>
        /// Resolves the name of the property.
        /// </summary>
        protected override string ResolvePropertyName(string propertyName)
        {
            if (propertyName == "Id")
            {
                return "@id";
            }

            return base.ResolvePropertyName(propertyName);
        }
    }
}
