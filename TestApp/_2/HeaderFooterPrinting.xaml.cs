using System;
using System.Windows;
using XpsPrinting;
using XpsPrinting.Documents;
using XpsPrinting.Formatting.FlowDocumentBasedFormatting;
using XpsPrinting.Paging;

namespace TestApp._2
{
    public partial class HeaderFooterPrinting : Window
    {
        private int _count = 1;
        
        public HeaderFooterPrinting()
        {
            InitializeComponent();
            UpdateTextBox();
        }

        private void UpdateTextBox()
        {
            txtCount.Text = _count.ToString();
        }

        private void IncreaseClick(object sender, RoutedEventArgs e)
        {
            _count++;
            UpdateTextBox();
        }

        private void DecreaseClick(object sender, RoutedEventArgs e)
        {
            if (_count <= 1) return;
            _count--;
            UpdateTextBox();
        }

        private void GoClick(object sender, RoutedEventArgs e)
        {
            var blockFormatterFactory = new BlockFormattersFactory();
            var pm = new PrintManager();

            var formatter = new SimpleFlowDocumentDataFormatter();

            for (int i = 0; i < _count; i++)
            {
                string headerText = String.Format("Page {0} title", i);
                formatter.AppendBlocks(blockFormatterFactory.HeaderFromNewPage(headerText));
            }

            Func<int, IBlankPage> pageFactoryMethod = pageNum => new SimpleBlankPage
                                                              {
                                                                  Header = "Header/Footer printing sample",
                                                                  Footer = String.Format("Page number: {0}", pageNum+1)
                                                              };
            var blankPageSource = new RelayedBlankPageSource(pageFactoryMethod);

            var doc = new SimpleDocument(blankPageSource, formatter);
            pm.PrintPreview(doc);

        }
    }
}
