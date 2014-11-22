using FakeItEasy;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Nancy.RDF.Tests.Bindings
{
    [Binding]
    public class ModelSerializingSteps<T>
    {
        private readonly SerializationContext _context;

        public ModelSerializingSteps(SerializationContext context)
        {
            _context = context;
        }

        [Given(@"A model with content:")]
        public void GivenAModelOfType(Table table)
        {
            ScenarioContext.Current["model"] = table.CreateInstance<T>();
        }

        [Given(@"@context is:")]
        public void GivenContextForIs(string resource)
        {
            A.CallTo(() => _context.ContextProvider.GetContext<T>()).Returns(JToken.Parse(resource));
        }
    }
}
