using Nancy.Bootstrapper;
using Nancy.RDF.Responses;

namespace Nancy.RDF
{
    /// <summary>
    /// Optional setup for serving RDF
    /// </summary>
    public static class RdfResponses
    {
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
