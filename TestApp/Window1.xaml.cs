using System.Data;
using System.Windows;

namespace TestApp
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            DataContext = new LogViewModel(new DataView(Database.GetLogData()));
        }
    }
}