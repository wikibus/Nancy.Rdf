using CsQuery.ExtensionMethods;
using FakeItEasy;
using FluentAssertions;
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
        public void Should_serve_jsonld_context_by_default()
        {
            // given
            const string expected = "{ 'sch': 'http://schema.org' }";

            // when
            var response = _browser.Get("/context/someclass");

            // then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            JToken.DeepEquals(response.ToJSON(), expected).Should().BeTrue();
        }

        [Test]
        public void Should_not_serve_jsonld_context_in_other_format()
        {
            // when
            var response = _browser.Get("/context/someclass", with => with.Accept(new MediaRange(RdfSerialization.Turtle.MediaType)));

            // then
            response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
        }

        public class ContextModuleTestable : ContextModule
        {
            public ContextModuleTestable() : base("context")
            {
            }
        }
    }
}
