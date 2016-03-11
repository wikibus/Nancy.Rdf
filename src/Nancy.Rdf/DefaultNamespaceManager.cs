using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NullGuard;

namespace Nancy.Rdf
{
    /// <summary>
    /// Dictionary-backed <see cref="INamespaceManager"/>
    /// </summary>
    internal class DictionaryNamespaceManager : INamespaceManager
    {
        private readonly IDictionary<string, Uri> _namespaces;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryNamespaceManager"/> class.
        /// </summary>
        public DictionaryNamespaceManager()
        {
            _namespaces = new Dictionary<string, Uri>();
        }

        /// <inheritdoc />
        public Uri BaseUri { get; private set; }

        /// <inheritdoc />
        public IEnumerator<NamespaceMap> GetEnumerator()
        {
            return _namespaces.Select(p => new NamespaceMap(p.Key, p.Value)).GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void AddNamespace(string prefix, Uri ns)
        {
            _namespaces.Add(prefix, ns);
        }

        /// <inheritdoc />
        public void SetBaseUri([AllowNull] Uri baseUri)
        {
            BaseUri = baseUri;
        }
    }
}
