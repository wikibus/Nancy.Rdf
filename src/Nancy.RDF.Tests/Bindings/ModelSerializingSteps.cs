using FakeItEasy;
using Nancy.RDF.Tests.Models;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Nancy.RDF.Tests.Bindings
{
    [Binding]
    public class ModelSerializingSteps
    {
        private readonly SerializationContext _context;

        protected ModelSerializingSteps(SerializationContext context)
        {
            _context = context;
        }

        [Given(@"A model with content:"), Scope(Tag = "Brochure")]
        public void GivenABrochure(Table table)
        {
            ScenarioContext.Current["model"] = table.CreateInstance<Brochure>();
        }

        [Given(@"@context is:"), Scope(Tag = "Brochure")]
        public void GivenBrochureContext(string resource)
        {
            A.CallTo(() => _context.ContextProvider.GetContext(typeof(Brochure))).Returns(JToken.Parse(resource));
        }
    }
}
