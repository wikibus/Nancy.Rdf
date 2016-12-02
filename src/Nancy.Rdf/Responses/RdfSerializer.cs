using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using JsonLD.Core;
using JsonLD.Entities;
using Nancy.IO;
using Nancy.Responses.Negotiation;
using VDS.RDF;

namespace Nancy.Rdf.Responses
{
    /// <summary>
    /// Serializer for RDF data types (other than JSON-LD)
    /// </summary>
    public abstract class RdfSerializer : IRdfSerializer
    {
        private readonly RdfSerialization serialization;
        private readonly INodeFactory nodeFactory;
        private readonly IEntitySerializer entitySerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RdfSerializer" /> class.
        /// </summary>
        /// <param name="serialization">The output serialization.</param>
        /// <param name="entitySerializer">The entity serializer.</param>
        protected RdfSerializer(RdfSerialization serialization, IEntitySerializer entitySerializer)
        {
            this.serialization = serialization;
            this.entitySerializer = entitySerializer;

            this.nodeFactory = new NodeFactory();
        }

        /// <inheritdoc />
        public IEnumerable<string> Extensions
        {
            get { yield return this.serialization.Extension; }
        }

        /// <inheritdoc />
        public virtual bool CanSerialize(MediaRange contentType)
        {
            return this.serialization.MediaType.Equals(contentType, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <inheritdoc />
        public void Serialize<TModel>(MediaRange contentType, TModel model, Stream outputStream)
        {
            WrappedModel? wrappedModel = model as WrappedModel?;
            var actualModel = wrappedModel == null ? model : wrappedModel.Value.Model;

            using (var writer = new StreamWriter(new UnclosableStreamWrapper(outputStream)))
            {
                var javascriptObject = this.entitySerializer.Serialize(actualModel);
                if (wrappedModel != null)
                {
                    javascriptObject.AddBaseToContext(wrappedModel.Value.BaseUrl);
                }

                var rdf = (RDFDataset)JsonLdProcessor.ToRDF(javascriptObject);

                this.WriteRdf(writer, rdf.GetQuads("@default").Select(this.ToTriple));
            }
        }

        /// <summary>
        /// Writes the RDF is proper serialization.
        /// </summary>
        protected abstract void WriteRdf(StreamWriter writer, IEnumerable<Triple> triples);

        private INode CreateNode(RDFDataset.Node node)
        {
            if (node.IsIRI())
            {
                return this.nodeFactory.CreateUriNode(new Uri(node.GetValue()));
            }

            if (node.IsBlankNode())
            {
                return this.nodeFactory.CreateBlankNode(node.GetValue());
            }

            var literal = node.GetValue();
            var datatype = new Uri(node.GetDatatype());

            return this.nodeFactory.CreateLiteralNode(literal, datatype);
        }

        private Triple ToTriple(RDFDataset.Quad triple)
        {
            var subj = this.CreateNode(triple.GetSubject());
            var pred = this.CreateNode(triple.GetPredicate());
            var obj = this.CreateNode(triple.GetObject());

            return new Triple(subj, pred, obj);
        }
    }
}
