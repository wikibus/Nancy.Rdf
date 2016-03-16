using System;
using System.IO;
using FakeItEasy;
using FluentAssertions;
using JsonLD.Entities;
using Nancy.Rdf.Contexts;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using JsonLdSerializer = Nancy.Rdf.Responses.JsonLdSerializer;

namespace Nancy.Rdf.Tests
{
    [TestFixture]
    public class JsonLdSerializerTests
    {
        private readonly JObject _modelWithContext = JObject.Parse("{ '@context': { 'ex': 'http://example.com' } }");

        private JsonLdSerializer _serializer;
        private IEntitySerializer _entitySerializer;
        private IContextPathMapper _pathMapper;

        [SetUp]
        public void Setup()
        {
            _entitySerializer = A.Fake<IEntitySerializer>();
            _pathMapper = A.Fake<IContextPathMapper>();
            _serializer = new JsonLdSerializer(_entitySerializer, _pathMapper);
        }

        [Test]
        public void Should_replace_context_when_available()
        {
            // given
            const string expectedUri = "http://example.com/contexts/model";
            A.CallTo(() => _pathMapper.BaseContextUrl).Returns(new Uri("http://example.com/contexts"));
            A.CallTo(() => _pathMapper.Contexts).Returns(new[] { new ContextPathMap("model", typeof(object)) });
            A.CallTo(() => _entitySerializer.Serialize(A<object>.Ignored, null)).Returns(_modelWithContext);
            var outStream = new MemoryStream();

            // when
            _serializer.Serialize("content/type", new object(), outStream);

            // then
            outStream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(outStream))
            {
                var jObject = JObject.Parse(reader.ReadToEnd());

                JToken.DeepEquals(jObject[JsonLdKeywords.Context], expectedUri).Should().BeTrue();
            }
        }
    }
}
