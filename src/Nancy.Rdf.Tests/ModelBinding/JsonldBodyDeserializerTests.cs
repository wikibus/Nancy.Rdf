using System.IO;
using System.Text;
using FakeItEasy;
using FluentAssertions;
using JsonLD.Entities;
using Nancy.ModelBinding;
using Nancy.Rdf.ModelBinding;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Nancy.Rdf.Tests.ModelBinding
{
    public class JsonldBodyDeserializerTests
    {
        [Test]
        public void Should_support_jsonld()
        {
            // given
            var entitySerializer = A.Fake<IEntitySerializer>();
            var deserializer = new JsonldBodyDeserializer(entitySerializer);

            // then
            deserializer.CanDeserialize(RdfSerialization.JsonLd.MediaType, new BindingContext())
                .Should().BeTrue();
        }

        [Test]
        public void Deserialize_should_deserialize_JSONLD_proper_type_using_entity_serializer()
        {
            // given
            var entitySerializer = A.Fake<IEntitySerializer>();
            var binder = new JsonldBodyDeserializer(entitySerializer);
            var bindingContext = new BindingContext { DestinationType = typeof(TestModel) };
            var body = new MemoryStream(Encoding.UTF8.GetBytes("{}"));

            // when
            binder.Deserialize(RdfSerialization.JsonLd.MediaType, body, bindingContext);

            // then
            A.CallTo(() => entitySerializer.Deserialize<TestModel>(A<JToken>._)).MustHaveHappened();
        }

        private class TestModel
        {
        }
    }
}