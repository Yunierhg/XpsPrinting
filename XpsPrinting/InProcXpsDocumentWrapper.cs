using System;
using System.IO;
using System.IO.Packaging;
using System.Windows.Xps.Packaging;

namespace XpsPrinting
{
    public class InProcXpsDocumentWrapper : IDisposable
    {
        private readonly MemoryStream _memoryStream;
        private readonly Uri _documentUri;
        private bool _disposed = false;

        public InProcXpsDocumentWrapper()
        {
            _memoryStream = new MemoryStream();
            var package = Package.Open(_memoryStream, FileMode.Create, FileAccess.ReadWrite);
            _documentUri = new Uri("pack://" + Guid.NewGuid() + ".xps");
            PackageStore.AddPackage(_documentUri, package);
            Document = new XpsDocument(package, CompressionOption.NotCompressed, _documentUri.AbsoluteUri);
        }

        public XpsDocument Document { get; private set; }

        ~InProcXpsDocumentWrapper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                GC.SuppressFinalize(this);
                _memoryStream.Close();
                PackageStore.RemovePackage(_documentUri);
            }
        }
    }
}