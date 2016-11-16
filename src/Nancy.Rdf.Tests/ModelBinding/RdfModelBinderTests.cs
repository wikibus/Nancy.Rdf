using System.IO;
using System.Text;
using FakeItEasy;
using FluentAssertions;
using JsonLD.Entities;
using Nancy.ModelBinding;
using Nancy.Rdf.ModelBinding;
using Nancy.Responses.Negotiation;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Nancy.Rdf.Tests.ModelBinding
{
    public class RdfModelBinderTests
    {
        [Test]
        public void CanDeserialize_should_return_true_supported_media_type()
        {
            // given
            var binder = new RdfBodyDeserializerTestable(A.Fake<IEntitySerializer>());

            // when
            var canDeserialize = binder.CanDeserialize(RdfSerialization.NTriples.MediaType, new BindingContext());

            // then
            canDeserialize.Should().BeTrue();
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

        [Test]
        public void Deserialize_should_deserialize_directly_from_ntriples()
        {
            // given
            var entitySerializer = A.Fake<IEntitySerializer>();
            var binder = new NtriplesBodyDeserializer(entitySerializer);
            var bindingContext = new BindingContext { DestinationType = typeof(TestModel) };
            const string bodyString = "some nquads";
            var body = new MemoryStream(Encoding.UTF8.GetBytes(bodyString));

            // when
            binder.Deserialize(RdfSerialization.NTriples.MediaType, body, bindingContext);

            // then
            A.CallTo(() => entitySerializer.Deserialize<TestModel>(bodyString)).MustHaveHappened();
        }

        private class RdfBodyDeserializerTestable : RdfBodyDeserializer
        {
            public RdfBodyDeserializerTestable(IEntitySerializer entitySerializer)
                : base(RdfSerialization.NTriples, entitySerializer)
            {
            }

            public override object Deserialize(MediaRange contentType, Stream body, BindingContext context)
            {
                throw new System.NotImplementedException();
            }
        }

        private class TestModel
        {
        }
    }
}
