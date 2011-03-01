using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Documents;
using XpsPrinting.Formatting.ContentTemplates;
using XpsPrinting.Formatting.Tables;

namespace XpsPrinting.Formatting
{
    public class SimpleTitledTableDataFormatter : DocumentPaginatorDataFormatter
    {
        private readonly DataView _data;
        private readonly IEnumerable<PrintColumnInfo> _columnsInfo;
        private readonly string _title;

        public SimpleTitledTableDataFormatter(DataView data, IEnumerable<PrintColumnInfo> columnsInfo, string title)
        {
            _data = data;
            _columnsInfo = columnsInfo;
            _title = title;
        }

        protected override DocumentPaginator GetDocumentPaginator(Size maxPageSize)
        {
            var tblFormatter = new TableFormatter();
            var table = tblFormatter.FormatData(_data, _columnsInfo, maxPageSize.Width);

            var doc = new TitledBlockContentTemplate
                          {
                              Title = _title,
                              PageHeight = maxPageSize.Height,
                              PageWidth = maxPageSize.Width,
                              ColumnWidth = maxPageSize.Width,
                              PagePadding = new Thickness(0)
                          };

            doc.AppendBlock(table);

            return ((IDocumentPaginatorSource) doc).DocumentPaginator;
        }
    }
}