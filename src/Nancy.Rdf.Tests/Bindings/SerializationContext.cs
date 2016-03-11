using System.IO;
using FakeItEasy;
using JsonLD.Entities;

namespace Nancy.Rdf.Tests.Bindings
{
    public class SerializationContext
    {
        private readonly Stream _outputStream = new MemoryStream();
        private readonly IEntitySerializer _serializer = A.Fake<IEntitySerializer>();

        public Stream OutputStream
        {
            get { return _outputStream; }
        }

        public IEntitySerializer Serializer
        {
            get { return _serializer; }
        }

        public dynamic Result { get; set; }

        public string AcceptHeader { get; set; }
    }
}
