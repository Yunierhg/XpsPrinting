using System;
using System.Windows;
using System.Windows.Input;

namespace TestApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = this;
            RunDemo = new RelayCommand(ExecuteDemo, o => o != null && o is Type);
            InitializeComponent();
        }

        private void ExecuteDemo(object obj)
        {
            var demoWindowType = (Type) obj;
            var ctorInfo = demoWindowType.GetConstructor(new Type[0]);
            var window = (Window)ctorInfo.Invoke(new object[0]);
            window.ShowDialog();
        }

        public ICommand RunDemo { get; set; }
    }
}
