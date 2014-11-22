using System.IO;
using FakeItEasy;
using Nancy.RDF.Responses;

namespace Nancy.RDF.Tests.Bindings
{
    public class SerializationContext
    {
        private readonly Stream _outputStream = new MemoryStream();
        private readonly IContextProvider _contextProvider = A.Fake<IContextProvider>();

        public Stream OutputStream
        {
            get { return _outputStream; }
        }

        public IContextProvider ContextProvider
        {
            get { return _contextProvider; }
        }
    }
}
