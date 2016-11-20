using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.Rdf.Conventions;
using Nancy.Rdf.Responses;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Nancy.Rdf.Tests.Bindings
{
    [Binding]
    public class AcceptHeaderCoercionSteps
    {
        private CoerceAcceptHeaders convention;
        private IEnumerable<Tuple<string, decimal>> coercedAcceptHeaders;

        [Given(@"default convention")]
        public void GivenDefaultConvention()
        {
            this.convention = AcceptHeaderConventions.CoerceBlankAcceptHeader();
        }

        [Given(@"convention set to return json-ld")]
        public void GivenConventionSetToReturn()
        {
            this.convention = AcceptHeaderConventions.CoerceBlankAcceptHeader(RdfSerialization.JsonLd);
        }

        [When(@"invoke the convention with empty header collection")]
        public void WhenInvokeTheConvention()
        {
            this.coercedAcceptHeaders = this.convention(new Tuple<string, decimal>[0], new NancyContext());
        }

        [When(@"invoke the convention with non-empty header collection")]
        public void WhenInvokeTheConventionWithExistingHeader()
        {
            this.coercedAcceptHeaders = this.convention(new[] { Tuple.Create("text/html", 0.9m) }, new NancyContext());
        }

        [Then(@"Accept header should be '(.*)', '(.*)'")]
        public void ThenAcceptHeaderShouldBe(string mediaType, decimal weight)
        {
            Assert.That(this.coercedAcceptHeaders.Single(), Is.EqualTo(Tuple.Create(mediaType, weight)));
        }
    }
}
