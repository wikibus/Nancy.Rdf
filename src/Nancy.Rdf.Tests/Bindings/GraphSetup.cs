using BoDi;
using TechTalk.SpecFlow;
using VDS.RDF;

namespace Nancy.Rdf.Tests.Bindings
{
    [Binding]
    public class GraphSetup
    {
        private readonly IObjectContainer _container;

        public GraphSetup(IObjectContainer container)
        {
            _container = container;
        }

        [BeforeScenario]
        public void InitializeGraph()
        {
            _container.RegisterInstanceAs<IGraph>(new Graph());
        }
    }
}
