using System;

namespace Nancy.Rdf
{
    /// <summary>
    /// A prefix-namespace mapping
    /// </summary>
    public struct NamespaceMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceMap"/> struct.
        /// </summary>
        public NamespaceMap(string prefix, Uri ns)
            : this()
        {
            Namespace = ns;
            Prefix = prefix;
        }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        public string Prefix { get; private set; }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        public Uri Namespace { get; private set; }
    }
}
