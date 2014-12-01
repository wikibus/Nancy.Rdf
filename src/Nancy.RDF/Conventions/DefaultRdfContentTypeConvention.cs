using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.RDF.Responses;

namespace Nancy.RDF.Conventions
{
    /// <summary>
    /// Delegate for Nancy's Accept header coercion convention
    /// </summary>
    /// <param name="existingHeaders">The existing headers.</param>
    /// <param name="context">The Nancy context.</param>
    /// <returns>modified Accept headers</returns>
    public delegate IEnumerable<Tuple<string, decimal>> CoerceAcceptHeaders(
        IEnumerable<Tuple<string, decimal>> existingHeaders,
        NancyContext context);

    /// <summary>
    /// Provides Nancy.Rdf Accept header coercion conventions
    /// </summary>
    public static class DefaultRdfContentTypeConvention
    {
        /// <summary>
        /// Replaces blank accept header with Turtle.
        /// </summary>
        /// <param name="serialization">The RDF format to return by default.</param>
        /// <returns>the Accept header coercion convention</returns>
        public static CoerceAcceptHeaders CoerceBlankAcceptHeader(RdfSerialization serialization)
        {
            return (currentAcceptHeaders, context) =>
                {
                    var current = currentAcceptHeaders as Tuple<string, decimal>[] ?? currentAcceptHeaders.ToArray();

                    return !current.Any() ? new[] { Tuple.Create(serialization.MediaType, 1m) } : current;
                };
        }

        /// <summary>
        /// Replaces blank accept header with Turtle.
        /// </summary>
        /// <returns>the Accept header coercion convention</returns>
        public static CoerceAcceptHeaders CoerceBlankAcceptHeader()
        {
            return CoerceBlankAcceptHeader(RdfSerialization.Turtle);
        }
    }
}
