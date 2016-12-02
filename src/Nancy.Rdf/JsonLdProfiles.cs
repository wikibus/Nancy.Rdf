namespace Nancy.Rdf
{
    /// <summary>
    /// Predefined JSON-LD ACCEPT header profiles
    /// </summary>
    public static class JsonLdProfiles
    {
        /// <summary>
        /// Uri of expanded profile
        /// </summary>
        /// <remarks>when added to ACCEPT header forces the response to be an expanded document</remarks>
        public const string Expanded = "http://www.w3.org/ns/json-ld#expanded";
    }
}
