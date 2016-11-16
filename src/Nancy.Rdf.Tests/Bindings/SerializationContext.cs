using System;
using System.IO;
using FakeItEasy;
using JsonLD.Entities;

namespace Nancy.Rdf.Tests.Bindings
{
    public class SerializationContext : IDisposable
    {
        private readonly IEntitySerializer _serializer = A.Fake<IEntitySerializer>();
        private Stream _outputStream = new MemoryStream();

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_outputStream != null)
                {
                    _outputStream.Dispose();
                    _outputStream = null;
                }
            }
        }
    }
}
