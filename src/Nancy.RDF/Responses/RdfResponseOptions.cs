namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Options for negotiating RDF responses
    /// </summary>
    public class RdfResponseOptions
    {
        /// <summary>
        /// Gets or sets the fallback serialization for wildcard requested media type.
        /// </summary>
        public RdfSerialization FallbackSerialization { get; set; }
    }
}
