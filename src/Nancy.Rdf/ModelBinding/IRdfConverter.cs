using System.IO;
using VDS.RDF;

namespace Nancy.Rdf.ModelBinding
{
    /// <summary>
    /// Converts RDF serializations to n-triples
    /// </summary>
    public interface IRdfConverter
    {
        /// <summary>
        /// Converts to nquads.
        /// </summary>
        /// <returns>input rdf serialized as n-triples</returns>
        string ConvertToNtriples(Stream rdf, IRdfReader reader);
    }
}