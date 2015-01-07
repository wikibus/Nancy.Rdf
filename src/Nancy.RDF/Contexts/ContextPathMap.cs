using System;

namespace Nancy.RDF.Contexts
{
    /// <summary>
    /// Maps a Nancy path to a model type
    /// </summary>
    public struct ContextPathMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextPathMap"/> struct.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="modelType">Type of the model.</param>
        public ContextPathMap(string path, Type modelType) : this()
        {
            Path = path;
            ModelType = modelType;
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        public Type ModelType { get; private set; }
    }
}
