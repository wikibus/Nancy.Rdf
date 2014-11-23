using Newtonsoft.Json.Serialization;

namespace Nancy.RDF
{
    /// <summary>
    /// Camel-case contract resolver with overrides for JSON-LD keywords
    /// </summary>
    public class JsonLdContractResolver : CamelCasePropertyNamesContractResolver
    {
        /// <summary>
        /// Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.
        /// </summary>
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);

            if (member.Name == "Id")
            {
                jsonProperty.PropertyName = "@id";
            }

            return jsonProperty;
        }
    }
}
