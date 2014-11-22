using System;
using NUnit.Framework;
using TechTalk.SpecFlow;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Query.Datasets;

namespace Nancy.RDF.Tests.Bindings
{
    [Binding]
    public class RdfGraphSteps
    {
        private readonly IGraph _graph;
        private readonly ISparqlQueryProcessor _queryProcessor;
        private readonly SparqlQueryParser _parser = new SparqlQueryParser();

        public RdfGraphSteps(IGraph graph)
        {
            _graph = graph;
            _queryProcessor = new LeviathanQueryProcessor(new InMemoryDataset(_graph));
        }

        [Then(@"graph should match:")]
        public void ThenGraphShouldMatch(string askQuery)
        {
            var query = _parser.ParseFromString(askQuery);
            Assert.That(query.QueryType, Is.EqualTo(SparqlQueryType.Ask), "Query must be ASK");

            _queryProcessor.ProcessQuery(query);
        }
    }
}
