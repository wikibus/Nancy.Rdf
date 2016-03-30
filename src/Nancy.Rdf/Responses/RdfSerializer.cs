using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JsonLD.Core;
using JsonLD.Entities;
using Nancy.IO;
using VDS.RDF;
using Vocab;

namespace Nancy.Rdf.Responses
{
    /// <summary>
    /// Serializer for RDF data types (other than JSON-LD)
    /// </summary>
    public abstract class RdfSerializer : IRdfSerializer
    {
        private readonly RdfSerialization _serialization;
        private readonly INodeFactory _nodeFactory;
        private readonly IEntitySerializer _entitySerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RdfSerializer" /> class.
        /// </summary>
        /// <param name="serialization">The output serialization.</param>
        /// <param name="entitySerializer">The entity serializer.</param>
        protected RdfSerializer(RdfSerialization serialization, IEntitySerializer entitySerializer)
        {
            _serialization = serialization;
            _entitySerializer = entitySerializer;

            _nodeFactory = new NodeFactory();
        }

        /// <inheritdoc />
        public IEnumerable<string> Extensions
        {
            get { yield return _serialization.Extension; }
        }

        /// <inheritdoc />
        public virtual bool CanSerialize(string contentType)
        {
            return _serialization.MediaType.Equals(contentType, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <inheritdoc />
        public void Serialize<TModel>(string contentType, TModel model, Stream outputStream)
        {
            var actualModel = model is WrappedModel ? ((WrappedModel)(object)model).Model : model;

            using (var writer = new StreamWriter(new UnclosableStreamWrapper(outputStream)))
            {
                var jsObject = _entitySerializer.Serialize(actualModel);
                var rdf = (RDFDataset)JsonLdProcessor.ToRDF(jsObject);

                WriteRdf(writer, rdf.GetQuads("@default").Select(ToTriple));
            }
        }

        /// <summary>
        /// Writes the RDF is proper serialization.
        /// </summary>
        protected abstract void WriteRdf(StreamWriter writer, IEnumerable<Triple> triples);

        [Obsolete("Remove when json-ld.net is fixed")]
        private static string FixSerializedValue(string literal, Uri datatype)
        {
            switch (datatype.ToString())
            {
                case Xsd.boolean:
                    return literal.ToLower();
                case Xsd.dateTime:
                case Xsd.date:
                    DateTime dateTime;
                    if (DateTime.TryParse(literal, out dateTime))
                    {
                        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    break;
            }

            return literal;
        }

        private INode CreateNode(RDFDataset.Node node)
        {
            if (node.IsIRI())
            {
                return _nodeFactory.CreateUriNode(new Uri(node.GetValue()));
            }

            if (node.IsBlankNode())
            {
                return _nodeFactory.CreateBlankNode(node.GetValue());
            }

            var literal = node.GetValue();
            var datatype = new Uri(node.GetDatatype());

            literal = FixSerializedValue(literal, datatype);

            return _nodeFactory.CreateLiteralNode(literal, datatype);
        }

        private Triple ToTriple(RDFDataset.Quad triple)
        {
            var subj = CreateNode(triple.GetSubject());
            var pred = CreateNode(triple.GetPredicate());
            var obj = CreateNode(triple.GetObject());

            return new Triple(subj, pred, obj);
        }
    }
}
