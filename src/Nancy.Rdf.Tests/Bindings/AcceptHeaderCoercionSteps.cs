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
        private CoerceAcceptHeaders _convention;
        private IEnumerable<Tuple<string, decimal>> _coercedAcceptHeaders;

        [Given(@"default convention")]
        public void GivenDefaultConvention()
        {
            _convention = AcceptHeaderConventions.CoerceBlankAcceptHeader();
        }

        [Given(@"convention set to return json-ld")]
        public void GivenConventionSetToReturn()
        {
            _convention = AcceptHeaderConventions.CoerceBlankAcceptHeader(RdfSerialization.JsonLd);
        }

        [When(@"invoke the convention with empty header collection")]
        public void WhenInvokeTheConvention()
        {
            _coercedAcceptHeaders = _convention(new Tuple<string, decimal>[0], new NancyContext());
        }

        [When(@"invoke the convention with non-empty header collection")]
        public void WhenInvokeTheConventionWithExistingHeader()
        {
            _coercedAcceptHeaders = _convention(new[] { Tuple.Create("text/html", 0.9m) }, new NancyContext());
        }

        [Then(@"Accept header should be '(.*)', '(.*)'")]
        public void ThenAcceptHeaderShouldBe(string mediaType, decimal weight)
        {
            Assert.That(_coercedAcceptHeaders.Single(), Is.EqualTo(Tuple.Create(mediaType, weight)));
        }
    }
}
