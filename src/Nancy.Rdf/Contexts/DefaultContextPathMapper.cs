using System;
using System.Collections.Generic;

namespace Nancy.Rdf.Contexts
{
    /// <summary>
    /// Basic implementation of <see cref="IContextPathMapper"/>
    /// </summary>
    public class DefaultContextPathMapper : IContextPathMapper
    {
        private const string DefaultContextPath = "_contexts";
        private readonly NancyContext _context;
        private readonly IList<ContextPathMap> _contexts = new List<ContextPathMap>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultContextPathMapper"/> class.
        /// </summary>
        public DefaultContextPathMapper(NancyContext context)
        {
            _context = context;
            BasePath = DefaultContextPath;
        }

        /// <summary>
        /// Gets the base path at which @contexts will be served.
        /// </summary>
        public string BasePath { get; }

        /// <summary>
        /// Gets the context path maps.
        /// </summary>
        public virtual IEnumerable<ContextPathMap> Contexts
        {
            get { return _contexts; }
        }

        /// <summary>
        /// Gets the base <see cref="Uri" /> path at which @contexts will be served.
        /// </summary>
        public virtual Uri BaseContextUrl
        {
            get { return new Uri(_context.Request.Url.SiteBase + BasePath); }
        }

        /// <summary>
        /// Enables serving of type <typeparamref name="T" />'s @context. The class name will be used as path
        /// </summary>
        /// <typeparam name="T">model type</typeparam>
        protected void ServeContextOf<T>()
        {
            ServeContextOf<T>(typeof(T).Name);
        }

        /// <summary>
        /// Enables serving of type <typeparamref name="T" />'s @context.
        /// </summary>
        /// <typeparam name="T">model type</typeparam>
        /// <param name="path">The path to server the @context at.</param>
        protected void ServeContextOf<T>(string path)
        {
            _contexts.Add(new ContextPathMap(path, typeof(T)));
        }
    }
}
