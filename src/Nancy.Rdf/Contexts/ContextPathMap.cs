using System;
using NullGuard;

namespace Nancy.Rdf.Contexts
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
        public string Path { [return: AllowNull] get; private set; }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        public Type ModelType { [return: AllowNull] get; private set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        public static bool operator ==(ContextPathMap left, ContextPathMap right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        public static bool operator !=(ContextPathMap left, ContextPathMap right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Checks equality
        /// </summary>
        public bool Equals(ContextPathMap other)
        {
            return string.Equals(Path, other.Path) && ModelType == other.ModelType;
        }

        /// <summary>
        /// Checks equality
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is ContextPathMap && Equals((ContextPathMap)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Path != null ? Path.GetHashCode() : 0) * 397) ^ (ModelType != null ? ModelType.GetHashCode() : 0);
            }
        }
    }
}
