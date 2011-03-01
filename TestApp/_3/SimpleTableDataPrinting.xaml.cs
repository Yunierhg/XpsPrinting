using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Input;
using XpsPrinting;
using XpsPrinting.Documents;
using XpsPrinting.Formatting.Tables;
using XpsPrinting.Paging;

namespace TestApp._3
{
    public partial class SimpleTableDataPrinting : Window
    {
        private List<PrintColumnInfo> _printColumns;
        
        public SimpleTableDataPrinting()
        {
            InitializeComponent();
            Data = new DataView(GetLogData());
            PrintCommand = new RelayCommand(Print, CanPrint);
            PreviewCommand = new RelayCommand(p => Preview(), CanPreview);
            DataContext = this;
        }

        public DataView Data { get; private set; }

        public ICommand PrintCommand { get; private set; }

        public ICommand PreviewCommand { get; private set; }

        public List<PrintColumnInfo> PrintColumns
        {
            get
            {
                if (_printColumns != null) 
                    return _printColumns;
                _printColumns = new List<PrintColumnInfo>
                                    {
                                        new PrintColumnInfo("Order", "Order", PrintLength.Auto),
                                        new PrintColumnInfo("User", "User", PrintLength.Auto),
                                        new PrintColumnInfo("Object", "Object", PrintLength.Auto),
                                        new PrintColumnInfo("Message", "Message", new PrintLength(1, PrintUnitType.Star)),
                                        new PrintColumnInfo("OldValue", "OldValue", PrintLength.Auto),
                                        new PrintColumnInfo("NewValue", "NewValue", PrintLength.Auto),
                                        new PrintColumnInfo("Date", "Date", PrintLength.Auto)
                                    };
                return _printColumns;
            }
        }

        private void Print(object parameter)
        {
            var printManager = new PrintManager();
            var doc = GetDocument(Data, PrintColumns, "ActivityLog");
            printManager.Print(doc, "ActivityLog printing job");
        }

        private bool CanPrint(object parameter)
        {
            return Data != null;
        }

        public void Preview()
        {
            var printManager = new PrintManager();
            var doc = GetDocument(Data, PrintColumns, "ActivityLog");
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

        private static DataTable GetLogData()
        {
            const int startDataCount = 50;
            var dtLogs = new DataTable("ActivityLog");
            dtLogs.Columns.Add("Order");
            dtLogs.Columns.Add("User");
            dtLogs.Columns.Add("Object");
            dtLogs.Columns.Add("Message");
            dtLogs.Columns.Add("OldValue");
            dtLogs.Columns.Add("NewValue");
            dtLogs.Columns.Add("Date");

            for (int i = 0; i < startDataCount; i++)
            {
                DataRow currentRow = dtLogs.NewRow();
                currentRow["Order"] = i;
                currentRow["User"] = "UserName";
                currentRow["Object"] = i % 3 == 0 ? "Workspace" : "Project";
                currentRow["Message"] = i % 5 == 0 ? "Short Message" :
                                                                       "Very very long message to ensure that Grid and Printing works correctly and word wraps works too";
                currentRow["OldValue"] = currentRow["NewValue"] = i % 10 == 0 ? "Value" : string.Empty;
                currentRow["Date"] = DateTime.Now;
                dtLogs.Rows.Add(currentRow);
            }
            return dtLogs;
        }
    }
}