using System.IO;
using System.Reflection;
using JsonLD.Entities;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using StringWriter = System.IO.StringWriter;

namespace Nancy.Rdf.ModelBinding
{
    /// <summary>
    /// Converts body, first converting it to NQuads
    /// </summary>
    public abstract class NonJsonLdRdfBodyDeserializer : RdfBodyDeserializer
    {
        private static readonly MethodInfo DeserializeNquadsMethod = typeof(IEntitySerializer).GetMethod("Deserialize", new[] { typeof(string) });

        private static readonly IRdfWriter RdfWriter = new NTriplesWriter(NTriplesSyntax.Rdf11);
        private readonly IRdfReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="NonJsonLdRdfBodyDeserializer"/> class.
        /// </summary>
        protected NonJsonLdRdfBodyDeserializer(
            RdfSerialization serialization,
            IEntitySerializer serializer,
            IRdfReader reader)
            : base(serialization, serializer)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Deserialize the request body to a model
        /// </summary>
        public override object Deserialize(MediaRange contentType, Stream body, BindingContext context)
        {
            var deserialize = DeserializeNquadsMethod.MakeGenericMethod(context.DestinationType);

            return deserialize.Invoke(this.Serializer, new object[] { this.GetNquads(body) });
        }

        /// <summary>
        /// Converts body to N-Triples
        /// </summary>
        protected virtual string GetNquads(Stream body)
        {
            // todo: implement actual parsers for json-ld.net so that it's not necessary to parse and write to ntriples
            IGraph g = new Graph();

            using (var streamReader = new StreamReader(body))
            {
                this.reader.Load(g, streamReader);
            }

            using (var stringWriter = new StringWriter())
            {
                RdfWriter.Save(g, stringWriter);
                return stringWriter.ToString();
            }
        }
    }
}
