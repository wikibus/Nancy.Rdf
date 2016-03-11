using System;
using System.Collections.Generic;

namespace Nancy.RDF.Contexts
{
    /// <summary>
    /// Used to map JSON-LD contexts to Nancy resource paths
    /// </summary>
    public interface IContextPathMapper
    {
        /// <summary>
        /// Gets the base relative path at which @contexts will be served.
        /// </summary>
        string BasePath { get; }

        /// <summary>
        /// Gets the context path maps.
        /// </summary>
        IEnumerable<ContextPathMap> Contexts { get; }

        /// <summary>
        /// Gets the application path.
        /// </summary>
        Uri AppPath { get; }
    }
}
