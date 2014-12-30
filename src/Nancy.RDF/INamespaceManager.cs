using System;
using System.Collections.Generic;

namespace Nancy.RDF
{
    /// <summary>
    /// Manages RDF prefixes and base URI
    /// </summary>
    public interface INamespaceManager : IEnumerable<NamespaceMap>
    {
        /// <summary>
        /// Gets the base URI.
        /// </summary>
        Uri BaseUri { get; }

        /// <summary>
        /// Adds the namespace.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="ns">The namespace Uri.</param>
        void AddNamespace(string prefix, Uri ns);

        /// <summary>
        /// Sets the base URI.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        void SetBaseUri(Uri baseUri);
    }
}
