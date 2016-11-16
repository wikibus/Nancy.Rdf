using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using JsonLD.Entities;
using Nancy.Rdf.Contexts;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Nancy.Rdf.Tests
{
    public class ContextModuleTests
    {
        private Browser _browser;
        private IContextPathMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = A.Fake<IContextPathMapper>();
            A.CallTo(() => _mapper.BasePath).Returns("context");
            A.CallTo(() => _mapper.Contexts).Returns(new[] { new ContextPathMap("staticString", typeof(Model)) });

            _browser = new Browser(
                with => with.Module<JsonLdContextModule>()
                            .Dependency(_mapper)
                            .Dependency(A.Dummy<IEntitySerializer>())
                            .Dependency(A.Dummy<IContextProvider>()));
        }

        [Test]
        public async void Should_serve_type_jsonld_context_by_default()
        {
            // given
            const string context = "{ 'sch': 'http://schema.org' }";
            string expected = string.Format("{{'@context': {0} }}", context);

            // when
            var response = await _browser.Get("/context/staticString");

            // then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            JToken.DeepEquals(JObject.Parse(response.Body.AsString()), JObject.Parse(expected)).Should().BeTrue();
        }

        [Test]
        public async void Should_not_serve_jsonld_context_in_other_format()
        {
            // when
            var response = await _browser.Get("/context/staticString", with => with.Accept(new MediaRange(RdfSerialization.Turtle.MediaType)));

            // then
            response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
        }

        [UsedImplicitly]
        private class Model
        {
            [UsedImplicitly]
            public static JObject Context
            {
                get { return JObject.Parse("{ 'sch': 'http://schema.org' }"); }
            }
        }
    }
}
