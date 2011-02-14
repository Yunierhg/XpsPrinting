using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Documents;
using XpsPrinting.Formatting.ContentTemplates;
using XpsPrinting.Formatting.Tables;

namespace XpsPrinting.Formatting
{
    public class SimpleTitledTableDocumentFormatter : FlowDocumentFixedPageSizeFormatter
    {
        private readonly DataView _data;
        private readonly IEnumerable<PrintColumnInfo> _columnsInfo;
        private readonly string _title;

        public SimpleTitledTableDocumentFormatter(DataView data, IEnumerable<PrintColumnInfo> columnsInfo, string title)
        {
            _data = data;
            _columnsInfo = columnsInfo;
            _title = title;
        }

        protected override FlowDocument GetDocument(Size pageSize)
        {
            var tblFormatter = new TableFormatter();
            var table = tblFormatter.FormatData(_data, _columnsInfo, pageSize.Width);

            var doc = new TitledBlockContentTemplate();
            doc.Title = _title;
            doc.AppendBlock(table);

            doc.PageHeight = pageSize.Height;
            doc.PageWidth = pageSize.Width;
            doc.ColumnWidth = pageSize.Width;
            doc.PagePadding = new Thickness(0);
            
            return doc;
        }
    }
}