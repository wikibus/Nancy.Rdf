using System;
using System.Collections.Generic;
using System.IO;
using FakeItEasy;
using FluentAssertions;
using JsonLD.Entities;
using Nancy.Rdf.Contexts;
using Nancy.Rdf.Responses;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using JsonLdSerializer = Nancy.Rdf.Responses.JsonLdSerializer;

namespace Nancy.Rdf.Tests
{
    [TestFixture]
    public class JsonLdSerializerTests
    {
        private static readonly JObject ModelWithContext = JObject.Parse("{ '@context': { 'ex': 'http://example.com' } }");
        private static readonly JObject ModelWithoutContext = JObject.Parse("{ 'some': 'model' }");

        private JsonLdSerializer serializer;
        private IEntitySerializer entitySerializer;
        private IContextPathMapper pathMapper;

        [SetUp]
        public void Setup()
        {
            this.entitySerializer = A.Fake<IEntitySerializer>();
            this.pathMapper = A.Fake<IContextPathMapper>();
            this.serializer = new JsonLdSerializer(this.entitySerializer, this.pathMapper);
        }

        [Test]
        public void Should_replace_context_when_available()
        {
            // given
            const string siteBase = "http://example.com/";
            const string expectedUri = "http://example.com/contexts/model";
            A.CallTo(() => this.pathMapper.Contexts).Returns(new[] { new ContextPathMap("model", typeof(object)) });
            A.CallTo(() => this.pathMapper.BasePath).Returns("contexts");
            A.CallTo(() => this.entitySerializer.Serialize(A<object>.Ignored, null)).Returns(ModelWithContext);
            var outStream = new MemoryStream();

            // when
            this.serializer.Serialize("content/type", new WrappedModel(new object(), siteBase), outStream);

            // then
            outStream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(outStream))
            {
                var jObject = JObject.Parse(reader.ReadToEnd());

                JToken.DeepEquals(jObject[JsonLdKeywords.Context], expectedUri).Should().BeTrue();
            }
        }

        [Test]
        [TestCaseSource(nameof(AddBaseTestCases))]
        public void Should_add_site_base_as_base_to_context(string siteBase, JObject model, JToken expectedContext)
        {
            // given
            var outStream = new MemoryStream();
            A.CallTo(() => this.entitySerializer.Serialize(A<object>.Ignored, null)).Returns(model);

            // when
            this.serializer.Serialize("content/type", new WrappedModel(new object(), siteBase), outStream);

            // then
            outStream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(outStream))
            {
                var jObject = JObject.Parse(reader.ReadToEnd());

                JToken.DeepEquals(jObject[JsonLdKeywords.Context], expectedContext).Should().BeTrue();
            }
        }

        private static IEnumerable<TestCaseData> AddBaseTestCases()
        {
            const string siteBase = "http://example.com/some/path/";
            const string siteBaseNoTrailing = "http://example.com/some/path";

            yield return new TestCaseData(
                siteBase,
                ModelWithoutContext.DeepClone(),
                JObject.Parse("{ '@base': 'http://example.com/some/path/' }"));

            yield return new TestCaseData(
                siteBase,
                ModelWithContext.DeepClone(),
                JToken.Parse("[ { '@base': 'http://example.com/some/path/' }, { 'ex': 'http://example.com' } ]"));

            yield return new TestCaseData(
                siteBaseNoTrailing,
                ModelWithoutContext.DeepClone(),
                JObject.Parse("{ '@base': 'http://example.com/some/path/' }"));

            yield return new TestCaseData(
                siteBaseNoTrailing,
                ModelWithContext.DeepClone(),
                JToken.Parse("[ { '@base': 'http://example.com/some/path/' }, { 'ex': 'http://example.com' } ]"));
        }
    }
}
