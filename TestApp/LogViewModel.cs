using System.Collections.Generic;
using System.Data;
using XpsPrinting;
using XpsPrinting.Formatting.Tables;

namespace TestApp
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
            printManager.Print(_data, PrintColumns, "ActivityLog");
        }

        private bool CanPrint(object parameter)
        {
            return _data != null && _data.Count > 0;
        }

        public void Preview()
        {
            var printManager = new PrintManager();
            printManager.PrintPreview(_data, PrintColumns, "ActivityLog");
        }

        private bool CanPreview(object parameter)
        {
            return CanPrint(parameter);
        }
    }
}