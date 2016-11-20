using BoDi;
using TechTalk.SpecFlow;
using VDS.RDF;

namespace Nancy.Rdf.Tests.Bindings
{
    [Binding]
    public class GraphSetup
    {
        private readonly IObjectContainer container;

        public GraphSetup(IObjectContainer container)
        {
            this.container = container;
        }

        [BeforeScenario]
        public void InitializeGraph()
        {
            this.container.RegisterInstanceAs<IGraph>(new Graph());
        }
    }
}
