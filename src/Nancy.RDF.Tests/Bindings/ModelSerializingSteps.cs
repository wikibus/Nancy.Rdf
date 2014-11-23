using System;
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

        [Given(@"A model of type '(.*)'")]
        public void GivenATypedModel(string typeName)
        {
            var modelType = Type.GetType(typeName, true);
            ScenarioContext.Current["model"] = Activator.CreateInstance(modelType);
        }

        [Given(@"Model has property Id set to '(.*)'")]
        public void GivenModelHasPropertyIdSetTo(string id)
        {
            ((dynamic)ScenarioContext.Current["model"]).Id = new Uri(id);
        }

        [Given(@"@context is:"), Scope(Tag = "Brochure")]
        public void GivenBrochureContext(string resource)
        {
            A.CallTo(() => _context.ContextProvider.GetContext(typeof(Brochure))).Returns(JToken.Parse(resource));
        }

        [Given(@"expanded @context is:"), Scope(Tag = "Brochure")]
        public void GivenExpandedContextForIs(string contextContent)
        {
            A.CallTo(() => _context.ContextProvider.GetExpandedContext(typeof(Brochure))).Returns(JObject.Parse(contextContent));
        }
    }
}
