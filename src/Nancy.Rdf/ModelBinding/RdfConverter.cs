using System.IO;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using StringWriter = System.IO.StringWriter;

namespace Nancy.Rdf.ModelBinding
{
    /// <summary>
    /// Converts RDF serializations to n-triples
    /// </summary>
    /// <seealso cref="Nancy.Rdf.ModelBinding.IRdfConverter" />
    public class RdfConverter : IRdfConverter
    {
        private static readonly NTriplesWriter NTriplesWriter = new NTriplesWriter(NTriplesSyntax.Rdf11);

        /// <summary>
        /// Converts to nquads.
        /// </summary>
        /// <returns>input rdf serialized as n-triples</returns>
        public string ConvertToNtriples(Stream rdf, IRdfReader reader)
        {
            // todo: implement actual parsers for json-ld.net so that it's not necessary to parse and write to ntriples
            IGraph g = new Graph();

            using (var streamReader = new StreamReader(rdf))
            {
                reader.Load(g, streamReader);
            }

            using (var stringWriter = new StringWriter())
            {
                NTriplesWriter.Save(g, stringWriter);
                return stringWriter.ToString();
            }
        }
    }
}