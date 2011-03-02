using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace XpsPrinting.Formatting.FlowDocumentBasedFormatting
{
    /// <summary>
    /// Supports only single-column document with straightforward layout, just from up to down.
    /// Content is provided with Blocks and appended to the bottom of current document.
    /// </summary>
    public class SimpleFlowDocumentDataFormatter : DocumentPaginatorDataFormatterBase
    {
        private readonly List<IBlockFormatter> _blocks = new List<IBlockFormatter>();

        public void AppendBlocks(params IBlockFormatter[] blockToAppend)
        {
            _blocks.AddRange(blockToAppend);
        }

        protected override DocumentPaginator GetDocumentPaginator(Size maxPageSize)
        {
            var document = new FlowDocument();
            document.PagePadding = new Thickness(0);
            document.PageWidth = maxPageSize.Width;
            document.PageHeight = maxPageSize.Height;
            document.ColumnWidth = maxPageSize.Width;

            var formattedBlocks = from b in _blocks
                                  select b.Format(maxPageSize.Width);
            document.Blocks.AddRange(formattedBlocks);

            return ((IDocumentPaginatorSource) document).DocumentPaginator;
        }
    }
}