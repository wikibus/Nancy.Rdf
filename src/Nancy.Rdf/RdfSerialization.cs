using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NullGuard;

namespace Nancy.Rdf
{
    /// <summary>
    /// Represents an RDF media type
    /// </summary>
    public struct RdfSerialization
    {
        private readonly string mediaType;
        private readonly string extension;

        private RdfSerialization(string mediaType, string extension)
        {
            this.mediaType = mediaType;
            this.extension = extension;
        }

        /// <summary>
        /// Gets all RDF serializations
        /// </summary>
        public static IEnumerable<RdfSerialization> All
        {
            get
            {
                yield return Turtle;
                yield return RdfXml;
                yield return JsonLd;
                yield return Notation3;
                yield return NTriples;
            }
        }

        /// <summary>
        /// Gets the turtle mime type.
        /// </summary>
        public static RdfSerialization Turtle
        {
            get
            {
                return new RdfSerialization("text/turtle", "ttl");
            }
        }

        /// <summary>
        /// Gets the RDF/XML mime type.
        /// </summary>
        public static RdfSerialization RdfXml
        {
            get
            {
                return new RdfSerialization("application/rdf+xml", "rdf");
            }
        }

        /// <summary>
        /// Gets the JSON LD media type.
        /// </summary>
        public static RdfSerialization JsonLd
        {
            get
            {
                return new RdfSerialization("application/ld+json", "jsonld");
            }
        }

        /// <summary>
        /// Gets the n3 media type.
        /// </summary>
        public static RdfSerialization Notation3
        {
            get
            {
                return new RdfSerialization("text/rdf+n3", "n3");
            }
        }

        /// <summary>
        /// Gets the NTriples media type.
        /// </summary>
        public static RdfSerialization NTriples
        {
            get
            {
                return new RdfSerialization("text/plain", "nt");
            }
        }

        /// <summary>
        /// Gets the type of the media.
        /// </summary>
        public string MediaType
        {
            get { return this.mediaType; }
        }

        /// <summary>
        /// Gets the extension.
        /// </summary>
        public string Extension
        {
            get { return this.extension; }
        }

#pragma warning disable 1591
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Operator or built-in override")]
        public static bool operator ==(RdfSerialization left, RdfSerialization right)
        {
            return left.Equals(right);
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Operator or built-in override")]
        public static bool operator !=(RdfSerialization left, RdfSerialization right)
        {
            return !left.Equals(right);
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Operator or built-in override")]
        public bool Equals(RdfSerialization other)
        {
            return string.Equals(this.mediaType, other.mediaType) && string.Equals(this.extension, other.extension);
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Operator or built-in override")]
        public override bool Equals([AllowNull] object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is RdfSerialization && this.Equals((RdfSerialization)obj);
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Operator or built-in override")]
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.mediaType.GetHashCode() * 397) ^ this.extension.GetHashCode();
            }
        }
#pragma warning restore

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return this.MediaType;
        }
    }
}
