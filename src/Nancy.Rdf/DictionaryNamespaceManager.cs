using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nancy.Rdf
{
    /// <summary>
    /// Dictionary-backed <see cref="INamespaceManager"/>
    /// </summary>
    internal class DictionaryNamespaceManager : INamespaceManager
    {
        private readonly IDictionary<string, Uri> namespaces;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryNamespaceManager"/> class.
        /// </summary>
        public DictionaryNamespaceManager()
        {
            this.namespaces = new Dictionary<string, Uri>();
        }

        /// <inheritdoc />
        public Uri BaseUri { get; private set; }

        /// <inheritdoc />
        public IEnumerator<NamespaceMap> GetEnumerator()
        {
            return this.namespaces.Select(p => new NamespaceMap(p.Key, p.Value)).GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc />
        public void AddNamespace(string prefix, Uri ns)
        {
            this.namespaces.Add(prefix, ns);
        }

        /// <inheritdoc />
        public void SetBaseUri(Uri baseUri)
        {
            this.BaseUri = baseUri;
        }
    }
}
