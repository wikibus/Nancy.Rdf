using System;
using NUnit.Framework;
using TechTalk.SpecFlow;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Query.Datasets;

namespace Nancy.Rdf.Tests.Bindings
{
    [Binding]
    public class RdfGraphSteps
    {
        private readonly IGraph graph;
        private readonly ISparqlQueryProcessor queryProcessor;
        private readonly SparqlQueryParser parser = new SparqlQueryParser();

        public RdfGraphSteps(IGraph graph)
        {
            this.graph = graph;
            this.queryProcessor = new LeviathanQueryProcessor(new InMemoryDataset(this.graph));
        }

        [Then(@"graph should match:")]
        public void ThenGraphShouldMatch(string askQuery)
        {
            var query = this.parser.ParseFromString(askQuery);
            Assert.That(query.QueryType, Is.EqualTo(SparqlQueryType.Ask), "Query must be ASK");

            this.queryProcessor.ProcessQuery(query);
        }
    }
}
