using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using XpsPrinting.Documents;

namespace XpsPrinting
{
    public class PrintManager
    {
        public void Print(IDocument document, string jobTitle)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() != true)
                return;

            DocumentPaginator documentPaginator = new XpsPrintingDocumentPaginator(document);
            printDialog.PrintDocument(documentPaginator, jobTitle);
        }

        public void PrintPreview(IDocument document)
        {
            DocumentPaginator documentPaginator = new XpsPrintingDocumentPaginator(document);
            using (var xpsWrapper = new InProcXpsDocumentWrapper())
            {
                XpsDocumentWriter xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsWrapper.Document);
                xpsWriter.Write(documentPaginator);

                ShowXpsPreview(xpsWrapper.Document);
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