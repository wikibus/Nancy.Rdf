using System;
using System.IO;
using FakeItEasy;
using JsonLD.Entities;

namespace Nancy.Rdf.Tests.Bindings
{
    public class SerializationContext : IDisposable
    {
        private readonly IEntitySerializer serializer = A.Fake<IEntitySerializer>();
        private Stream outputStream = new MemoryStream();

        public Stream OutputStream
        {
            get { return this.outputStream; }
        }

        public IEntitySerializer Serializer
        {
            get { return this.serializer; }
        }

        public dynamic Result { get; set; }

        public string AcceptHeader { get; set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.outputStream != null)
                {
                    this.outputStream.Dispose();
                    this.outputStream = null;
                }
            }
        }
    }
}
