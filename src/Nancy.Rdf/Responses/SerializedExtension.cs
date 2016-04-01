using System;
using JsonLD.Entities;
using JsonLD.Entities.Context;
using Newtonsoft.Json.Linq;

namespace Nancy.Rdf.Responses
{
    /// <summary>
    /// Extensions to work with serialized model
    /// </summary>
    internal static class SerializedExtension
    {
        /// <summary>
        /// Adds the base to serialized object's context.
        /// </summary>
        public static void AddBaseToContext(this JToken serialized, Uri siteBase)
        {
            AddBaseToContext((JObject)serialized, siteBase);
        }

        /// <summary>
        /// Adds the base to serialized object's context.
        /// </summary>
        public static void AddBaseToContext(JObject serialized, Uri siteBase)
        {
            var baseContext = new JObject
            {
                { JsonLdKeywords.Base, siteBase.ToString().TrimEnd('/') + '/' }
            };

            if (serialized[JsonLdKeywords.Context] != null)
            {
                serialized[JsonLdKeywords.Context] = baseContext.MergeWith(serialized[JsonLdKeywords.Context]);
            }
            else
            {
                serialized.AddFirst(new JProperty(JsonLdKeywords.Context, baseContext));
            }
        }
    }
}
