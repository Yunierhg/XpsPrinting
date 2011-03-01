using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using XpsPrinting;
using XpsPrinting.Documents;
using XpsPrinting.Formatting;
using XpsPrinting.Formatting.Tables;
using XpsPrinting.Paging;
using SimpleBlankPage = TestApp._2.SimpleBlankPage;

namespace TestApp._3
{
    internal class LogViewModel
    {
        private readonly DataView _data;
        private readonly RelayCommand _printCommand;
        private readonly RelayCommand _previewCommand;
        private List<PrintColumnInfo> _printColumns;

        public LogViewModel(DataView data)
        {
            _data = data;
            _printCommand = new RelayCommand(Print, CanPrint);
            _previewCommand = new RelayCommand(p => Preview(), CanPreview);
        }

        public DataView Data
        {
            get { return _data; }
        }

        #region Commands
        
        public RelayCommand PrintCommand
        {
            get { return _printCommand; }
        }

        public RelayCommand PreviewCommand
        {
            get { return _previewCommand; }
        } 

        #endregion

        public List<PrintColumnInfo> PrintColumns
        {
            get
            {
                if (_printColumns == null)
                {
                    _printColumns = new List<PrintColumnInfo>();
                    _printColumns.Add(new PrintColumnInfo("Order", "Order", PrintLength.Auto));
                    _printColumns.Add(new PrintColumnInfo("User", "User", PrintLength.Auto));
                    _printColumns.Add(new PrintColumnInfo("Object", "Object", PrintLength.Auto));
                    _printColumns.Add(new PrintColumnInfo("Message", "Message", new PrintLength(1, PrintUnitType.Star)));
                    _printColumns.Add(new PrintColumnInfo("OldValue", "OldValue", PrintLength.Auto));
                    _printColumns.Add(new PrintColumnInfo("NewValue", "NewValue", PrintLength.Auto));
                    _printColumns.Add(new PrintColumnInfo("Date", "Date", PrintLength.Auto));
                }
                return _printColumns;
            }
        }

        private void Print(object parameter)
        {
            var printManager = new PrintManager();
            var doc = GetDocument(_data, PrintColumns, "ActivityLog");
            printManager.Print(doc, "ActivityLog printing job");
        }

        private bool CanPrint(object parameter)
        {
            return _data != null;
        }

        public void Preview()
        {
            var printManager = new PrintManager();
            var doc = GetDocument(_data, PrintColumns, "ActivityLog");
            printManager.PrintPreview(doc);
        }

        private bool CanPreview(object parameter)
        {
            return CanPrint(parameter);
        }

        private static IDocument GetDocument(DataView data, IEnumerable<PrintColumnInfo> columnsInfo, string title)
        {
            var pageSize = new Size(8.5 * 96, 11 * 96);
            Func<int, IBlankPage> pageFactoryMethod = pageNum => new BlankPageBase
            {
                PageSize = pageSize,
            };
            var blankPageSource = new RelayedBlankPageSource(pageFactoryMethod);
            var docFormatter = new SimpleTitledTableDataFormatter(data, columnsInfo, title);

            return new SimpleDocument(blankPageSource, docFormatter);
        }
    }
}