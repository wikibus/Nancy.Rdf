using System.Collections.Generic;

namespace Nancy.Rdf.Contexts
{
    /// <summary>
    /// Basic implementation of <see cref="IContextPathMapper"/>
    /// </summary>
    public class DefaultContextPathMapper : IContextPathMapper
    {
        private const string DefaultContextPath = "_contexts";
        private readonly IList<ContextPathMap> _contexts = new List<ContextPathMap>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultContextPathMapper"/> class.
        /// </summary>
        public DefaultContextPathMapper()
        {
            BasePath = DefaultContextPath;
        }

        /// <summary>
        /// Gets the base path at which @contexts will be served.
        /// </summary>
        public virtual string BasePath { get; }

        /// <summary>
        /// Gets the context path maps.
        /// </summary>
        public IEnumerable<ContextPathMap> Contexts
        {
            get { return _contexts; }
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
