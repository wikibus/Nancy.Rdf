using System;

namespace Nancy.RDF
{
    /// <summary>
    /// Helper methods for handling URIs
    /// </summary>
    internal static class UriExtensions
    {
        /// <summary>
        /// Appends path to the URI.
        /// </summary>
        public static Uri Append(this Uri uri, string path)
        {
            return new Uri(string.Format("{0}/{1}", uri.ToString().TrimEnd('/'), path.Trim('/')));
        }
    }
}
