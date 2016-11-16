using Nancy.Bootstrapper;

namespace Nancy.Rdf
{
    /// <summary>
    /// Optional setup for serving RDF
    /// </summary>
    public static class RdfResponses
    {
        /// <summary>
        /// The key used to store default RDF serialization in Nancy's context
        /// </summary>
        internal const string FallbackSerializationKey = "__nrfs";

        /// <summary>
        /// Sets the default serialization for wildcard Accept header.
        /// </summary>
        public static void SetDefaultSerialization(IPipelines pipelines, RdfSerialization serialization)
        {
            pipelines.BeforeRequest.AddItemToEndOfPipeline(context =>
            {
                context.Items[FallbackSerializationKey] = serialization;
                return null;
            });
        }
    }
}
