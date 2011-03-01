using System.Data;
using System.Windows;

namespace TestApp._3
{
    public partial class SimpleTableDataPrinting : Window
    {
        public SimpleTableDataPrinting()
        {
            InitializeComponent();
            DataContext = new LogViewModel(new DataView(Database.GetLogData()));
        }
    }
}