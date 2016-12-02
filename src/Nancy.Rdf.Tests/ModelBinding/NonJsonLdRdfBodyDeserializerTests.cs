using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FakeItEasy;
using FluentAssertions;
using JsonLD.Entities;
using Nancy.ModelBinding;
using Nancy.Rdf.ModelBinding;
using Nancy.Rdf.Responses;
using NUnit.Framework;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace Nancy.Rdf.Tests.ModelBinding
{
    public class NonJsonLdRdfBodyDeserializerTests
    {
        private readonly BindingContext bindingContext = new BindingContext { DestinationType = typeof(TestModel) };

        [TestCaseSource(nameof(NonJsonLdSerializations))]
        public void Should_support_serialization(RdfSerialization serialization)
        {
            // given
            var deserializer = new NonJsonLdRdfBodyDeserializer(A.Fake<IEntitySerializer>());

            // when
            deserializer.CanDeserialize(serialization.MediaType, new BindingContext());
        }

        [Test]
        public void Deserialize_should_deserialize_directly_from_ntriples()
        {
            // given
            var entitySerializer = A.Fake<IEntitySerializer>();
            var binder = new NonJsonLdRdfBodyDeserializer(entitySerializer);
            const string bodyString = "some nquads";
            var body = new MemoryStream(Encoding.UTF8.GetBytes(bodyString));

            // when
            binder.Deserialize(RdfSerialization.NTriples.MediaType, body, this.bindingContext);

            // then
            A.CallTo(() => entitySerializer.Deserialize<TestModel>(bodyString)).MustHaveHappened();
        }

        [Test, Sequential]
        public void Deserialize_should_convert_to_ntriples(
            [ValueSource(nameof(NonJsonLdSerializations))] RdfSerialization serialization,
            [Values(typeof(Notation3Parser), typeof(TurtleParser), typeof(RdfXmlParser))] Type serializerType)
        {
            // given
            var converter = A.Fake<IRdfConverter>();
            var deserializer = new NonJsonLdRdfBodyDeserializer(A.Fake<IEntitySerializer>(), converter);
            var body = new MemoryStream();

            // when
            deserializer.Deserialize(serialization.MediaType, body, this.bindingContext);

            // then
            A.CallTo(() => converter.ConvertToNtriples(body, A<IRdfReader>.That.Matches(rr => rr.GetType() == serializerType))).MustHaveHappened();
        }

        public IEnumerable<RdfSerialization> NonJsonLdSerializations()
        {
            yield return RdfSerialization.Notation3;
            yield return RdfSerialization.Turtle;
            yield return RdfSerialization.RdfXml;
        }

        private class TestModel
        {
        }
    }
}