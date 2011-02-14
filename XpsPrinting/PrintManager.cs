using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using XpsPrinting.Formatting;
using XpsPrinting.Formatting.Tables;
using XpsPrinting.Paging;

namespace XpsPrinting
{
    public class PrintManager
    {
        public void Print(DataView data, IEnumerable<PrintColumnInfo> columnsInfo, string title)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() != true) 
                return;

            var pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
            DocumentPaginator p = GetPaginator(pageSize, data, columnsInfo, title);
            printDialog.PrintDocument(p, title);
        }

        public void PrintPreview(DataView data, IEnumerable<PrintColumnInfo> columnsInfo, string title)
        {
            //Page Size is Letter size 8.5 x 11 inches
            var pageSize = new Size(8.5*96, 11*96);

            DocumentPaginator documentPaginator = GetPaginator(pageSize, data, columnsInfo, title);
            using (var xpsWrapper = new InProcXpsDocumentWrapper())
            {
                XpsDocumentWriter xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsWrapper.Document);
                xpsWriter.Write(documentPaginator);

                ShowXpsPreview(xpsWrapper.Document);
            }
        }

        private static DocumentPaginator GetPaginator(Size pageSize, DataView data, IEnumerable<PrintColumnInfo> columnsInfo, string title)
        {
            Action<int, SimplePageTemplate> adjustPage = (pageNum, page) => page.PageNumber = String.Format("Page number: {0}", pageNum);

            var blankPageSource = new TypedBlankPageSource<SimplePageTemplate>(pageSize, adjustPage);
            var docFormatter = new SimpleTitledTableDocumentFormatter(data, columnsInfo, title);

            return new TemplatingPaginator(docFormatter, blankPageSource);
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