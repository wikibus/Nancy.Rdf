using FakeItEasy;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

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

        [Given(@"A serialized model:")]
        public void GivenASerializedModel(string json)
        {
            A.CallTo(() => _context.Serializer.Serialize(A<object>.Ignored)).Returns(JObject.Parse(json));
        }

        [Then(@"output stream should equal")]
        public void ThenOutputStreamShouldEqual(string expectedBody)
        {
            Assert.That(JToken.DeepEquals(_context.Result, JToken.Parse(expectedBody)));
        }
    }
}
