using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JsonLD.Entities;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;

namespace Nancy.Rdf.ModelBinding
{
    /// <summary>
    /// Deserializers request body, first converting it to n-triples
    /// </summary>
    public class NonJsonLdRdfBodyDeserializer : RdfBodyDeserializer
    {
        private static readonly MethodInfo DeserializeNquadsMethod = typeof(IEntitySerializer).GetMethod("Deserialize", new[] { typeof(string) });

        private readonly IDictionary<RdfSerialization, IRdfReader> readers;
        private readonly IRdfConverter converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="NonJsonLdRdfBodyDeserializer"/> class.
        /// </summary>
        public NonJsonLdRdfBodyDeserializer(IEntitySerializer serializer)
            : this(serializer, new RdfConverter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonJsonLdRdfBodyDeserializer"/> class.
        /// </summary>
        public NonJsonLdRdfBodyDeserializer(
            IEntitySerializer serializer,
            IRdfConverter converter)
            : this(serializer, RdfSerialization.RdfXml, RdfSerialization.NTriples, RdfSerialization.Notation3, RdfSerialization.Turtle)
        {
            this.converter = converter;
        }

        private NonJsonLdRdfBodyDeserializer(IEntitySerializer serializer, params RdfSerialization[] serializations)
            : base(serializer, serializations)
        {
            this.readers = new Dictionary<RdfSerialization, IRdfReader>
            {
                { RdfSerialization.Notation3, new Notation3Parser() },
                { RdfSerialization.RdfXml, new RdfXmlParser() },
                { RdfSerialization.Turtle, new TurtleParser() }
            };
        }

        /// <summary>
        /// Deserialize the request body to a model
        /// </summary>
        public override object Deserialize(MediaRange contentType, Stream body, BindingContext context)
        {
            var deserialize = DeserializeNquadsMethod.MakeGenericMethod(context.DestinationType);
            string nquads;

            if (contentType.Matches(RdfSerialization.NTriples.MediaType))
            {
                using (var bodyReader = new StreamReader(body))
                {
                    nquads = bodyReader.ReadToEnd();
                }
            }
            else
            {
                var reader = this.readers.First(r => contentType.Matches(r.Key.MediaType)).Value;
                nquads = this.converter.ConvertToNtriples(body, reader);
            }

            return deserialize.Invoke(this.Serializer, new object[] { nquads });
        }
    }
}
