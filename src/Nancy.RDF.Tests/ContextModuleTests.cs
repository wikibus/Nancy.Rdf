using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using JsonLD.Entities;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Nancy.RDF.Tests
{
    public class ContextModuleTests
    {
        private Browser _browser;

        [SetUp]
        public void Setup()
        {
            _browser = new Browser(
                with => with.Module(new ContextModuleTestable())
                            .Dependency(A.Dummy<IEntitySerializer>()));
        }

        [Test]
        public void Should_serve_statically_set_up_string_jsonld_context_by_default()
        {
            // given
            const string expected = "{ 'sch': 'http://schema.org' }";

            // when
            var response = _browser.Get("/context/staticString");

            // then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            JToken.DeepEquals(JObject.Parse(response.Body.AsString()), JObject.Parse(expected)).Should().BeTrue();
        }

        [Test]
        public void Should_serve_statically_set_up_jsonld_context_by_default()
        {
            // given
            const string expected = "{ 'sch': 'http://schema.org' }";

            // when
            var response = _browser.Get("/context/staticJObject");

            // then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            JToken.DeepEquals(JObject.Parse(response.Body.AsString()), JObject.Parse(expected)).Should().BeTrue();
        }

        [Test]
        public void Should_serve_type_jsonld_context_by_default()
        {
            // given
            const string expected = "{ 'sch': 'http://schema.org' }";

            // when
            var response = _browser.Get("/context/model");

            // then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            JToken.DeepEquals(JObject.Parse(response.Body.AsString()), JObject.Parse(expected)).Should().BeTrue();
        }

        [Test]
        public void Should_not_serve_jsonld_context_in_other_format()
        {
            // when
            var response = _browser.Get("/context/staticString", with => with.Accept(new MediaRange(RdfSerialization.Turtle.MediaType)));

            // then
            response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
        }

        private class ContextModuleTestable : JsonLdContextModule
        {
            public ContextModuleTestable() : base("context", A.Dummy<IContextProvider>())
            {
                Get["staticString"] = ServeContext("{ 'sch': 'http://schema.org' }");
                Get["staticJObject"] = ServeContext(JObject.Parse("{ 'sch': 'http://schema.org' }"));
                Get["model"] = ServeContextOf<Model>();
            }
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
