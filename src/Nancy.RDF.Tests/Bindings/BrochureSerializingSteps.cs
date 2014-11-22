using TechTalk.SpecFlow;

namespace Nancy.RDF.Tests.Bindings
{
    [Binding, Scope(Tag = "Brochure")]
    public class BrochureSerializingSteps<T> : ModelSerializingSteps<T>
    {
        public BrochureSerializingSteps(SerializationContext context) : base(context)
        {
        }
    }
}