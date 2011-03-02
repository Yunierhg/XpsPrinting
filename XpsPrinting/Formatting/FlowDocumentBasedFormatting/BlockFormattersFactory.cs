using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Documents;
using XpsPrinting.Formatting.FlowDocumentBasedFormatting.Tables;

namespace XpsPrinting.Formatting.FlowDocumentBasedFormatting
{
    public class BlockFormattersFactory
    {
        private readonly TableFormatter _tblFormatter = new TableFormatter();

        public IBlockFormatter Header(string headerText)
        {
            var run = new Run(headerText);
            run.FontSize = 18;
            run.FontWeight = FontWeights.Bold;

            var paragraph = new Paragraph(run);
            paragraph.TextAlignment = TextAlignment.Center;
            paragraph.Margin = new Thickness(10);
            return new RelayBlockFormatter(_ => paragraph);
        }

        public IBlockFormatter Table(DataView data, IEnumerable<PrintColumnInfo> columnsInfo, string title)
        {
            return new RelayBlockFormatter(width => _tblFormatter.FormatData(data, columnsInfo, width));
        }
    }
}