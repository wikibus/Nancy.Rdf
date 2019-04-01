namespace Nancy.Rdf.Sample
{
    public class ContextPathMapper : Contexts.DefaultContextPathMapper
    {
        public ContextPathMapper()
        {
            ServeContextOf<Person>();
        }
    }
}