using System;
using System.IO;
using FakeItEasy;
using JsonLD.Entities;

namespace Nancy.RDF.Tests.Bindings
{
    public class SerializationContext
    {
        private readonly Stream _outputStream = new MemoryStream();
        private readonly IContextProvider _contextProvider = A.Fake<IContextProvider>();
        private readonly IEntitySerializer _serializer;

        public SerializationContext()
        {
            A.CallTo(() => _contextProvider.GetContext(A<Type>.Ignored)).Returns(null);
            _serializer = new EntitySerializer(_contextProvider);
        }

        public Stream OutputStream
        {
            get { return _outputStream; }
        }

        public IContextProvider ContextProvider
        {
            get { return _contextProvider; }
        }

        public IEntitySerializer Serializer
        {
            get { return _serializer; }
        }
    }
}
