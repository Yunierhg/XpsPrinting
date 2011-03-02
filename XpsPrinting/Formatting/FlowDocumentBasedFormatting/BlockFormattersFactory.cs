using System;
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
            paragraph.KeepWithNext = true;
            return new RelayBlockFormatter(_ => paragraph);
        }

        public IBlockFormatter Text(string text)
        {
            string[] lines = text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            var section = new Section();

            foreach (var line in lines)
            {
                var paragraph = new Paragraph(new Run(line));
                paragraph.FontSize = 12;
                paragraph.TextIndent = 10;
                section.Blocks.Add(paragraph);
            }

            return new RelayBlockFormatter(_ => section);
        }

        public IBlockFormatter Table(DataView data, IEnumerable<PrintColumnInfo> columnsInfo, string title)
        {
            return new RelayBlockFormatter(width => _tblFormatter.FormatData(data, columnsInfo, width));
        }
    }
}