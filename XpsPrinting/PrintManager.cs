using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using XpsPrinting.Formatting;
using XpsPrinting.Paging;

namespace XpsPrinting
{
    public class PrintManager
    {
        public void Print(LogPrintDocumentCreator documentCreator, string documentDescription)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                var pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
                Func<Size, FlowDocument> documentConstructor = GetFlowDocumentConstructor(documentCreator);
                DocumentPaginator p = GetPaginator(pageSize, documentConstructor);
                printDialog.PrintDocument(p, documentDescription);
            }
        }

        private static DocumentPaginator GetPaginator(Size pageSize, Func<Size, FlowDocument> documentSource)
        {
            Action<int, SimplePageTemplate> adjustPage = (pageNum, page) => page.PageNumber = String.Format("Page number: {0}", pageNum);

            var blankPageSource = new TypedBlankPageSource<SimplePageTemplate>(pageSize, adjustPage);

            var dynamicPaginator = new FlowDocumentFixedPageSizeDynamicPaginator(documentSource);

            return new TemplatingPaginator(dynamicPaginator, blankPageSource);
        }


        private static Func<Size, FlowDocument> GetFlowDocumentConstructor(LogPrintDocumentCreator documentCreator)
        {
            return s =>
                       {
                           var doc = documentCreator.GetDocument(s, new Thickness(0));
                           doc.PageHeight = s.Height;
                           doc.PageWidth = s.Width;
                           doc.ColumnWidth = s.Width;
                           doc.PagePadding = new Thickness(0);
                           return doc;
                       };
        }

        public void PrintPreview(LogPrintDocumentCreator documentCreator, string documentDescription)
        {
            //Page Size is Letter size 8.5 x 11 inches
            var pageSize = new Size(8.5 * 96, 11 * 96);
            using (var memoryStream = new MemoryStream())
            {
                Package package = Package.Open(memoryStream, FileMode.Create, FileAccess.ReadWrite);
                var documentUri = new Uri("pack://" + documentDescription + ".xps");
                PackageStore.AddPackage(documentUri, package);
                try
                {
                    Func<Size, FlowDocument> documentConstructor = GetFlowDocumentConstructor(documentCreator);
                    DocumentPaginator documentPaginator = GetPaginator(pageSize, documentConstructor);

                    var xpsDocument = new XpsDocument(package, CompressionOption.NotCompressed, documentUri.AbsoluteUri);
                    XpsDocumentWriter xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
                    xpsWriter.Write(documentPaginator);

                    ShowXpsPreview(xpsDocument);
                }
                finally
                {
                    memoryStream.Close();
                    PackageStore.RemovePackage(documentUri);
                }
            }
        }

        private static void ShowXpsPreview(XpsDocument xpsDocument)
        {
            var previewWindow = new Window();
            var docViewer = new DocumentViewer();
            previewWindow.Content = docViewer;
            docViewer.Document = xpsDocument.GetFixedDocumentSequence();
            previewWindow.ShowDialog();
        }
    }
}