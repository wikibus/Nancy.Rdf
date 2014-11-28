using System;
using System.Collections.Generic;
using System.IO;
using JsonLD.Core;
using JsonLD.Entities;
using Nancy.IO;
using VDS.RDF;
using VDS.RDF.Parsing.Handlers;
using VDS.RDF.Writing.Formatting;

namespace Nancy.RDF.Responses
{
    /// <summary>
    /// Serializer for RDF data types (other than JSON-LD)
    /// </summary>
    public abstract class RdfSerializer : ISerializer
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
            using (var writer = new StreamWriter(new UnclosableStreamWrapper(outputStream)))
            {
                var jsObject = _entitySerializer.Serialize(model);
                var rdf = (RDFDataset)JsonLdProcessor.ToRDF(jsObject);

                var h = CreateHandler<TModel>(writer);
                h.StartRdf();

                foreach (var triple in rdf.GetQuads("@default"))
                {
                    var subj = CreateNode(triple.GetSubject());
                    var pred = CreateNode(triple.GetPredicate());
                    var obj = CreateNode(triple.GetObject());
                    h.HandleTriple(new Triple(subj, pred, obj));
                }

                h.EndRdf(true);
            }
        }

        /// <summary>
        /// Creates the RDF write handler.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="writer">The writer.</param>
        protected virtual IRdfHandler CreateHandler<TModel>(StreamWriter writer)
        {
            return new WriteThroughHandler(CreateFormatter(), writer);
        }

        /// <summary>
        /// Creates the triple formatter.
        /// </summary>
        protected abstract ITripleFormatter CreateFormatter();

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

            return _nodeFactory.CreateLiteralNode(node.GetValue(), new Uri(node.GetDatatype()));
        }
    }
}
