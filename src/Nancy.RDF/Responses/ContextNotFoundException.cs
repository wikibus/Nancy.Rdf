using System;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Represents errors which occur if @context cannot be found for a given type
    /// </summary>
    /// <typeparam name="T">entity type</typeparam>
    public class ContextNotFoundException<T> : Exception
    {
        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get { return string.Format("JSON-LD context not found for type {0}", typeof(T).FullName); }
        }
    }
}
