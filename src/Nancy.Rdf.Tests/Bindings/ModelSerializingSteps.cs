using FakeItEasy;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Nancy.Rdf.Tests.Bindings
{
    [Binding]
    public class ModelSerializingSteps
    {
        private readonly SerializationContext context;

        protected ModelSerializingSteps(SerializationContext context)
        {
            this.context = context;
        }

        [Given(@"A serialized model:")]
        public void GivenASerializedModel(string json)
        {
            A.CallTo(() => this.context.Serializer.Serialize(A<object>.Ignored, null)).Returns(JObject.Parse(json));
        }

        [Given(@"accepted media type '(.*)'")]
        public void GivenAcceptedMediaType(string mediaType)
        {
            this.context.AcceptHeader = mediaType;
        }

        [Then(@"output stream should equal")]
        public void ThenOutputStreamShouldEqual(string expectedBody)
        {
            var expected = JToken.Parse(expectedBody);
            Assert.That(JToken.DeepEquals(this.context.Result, expected));
        }
    }
}
