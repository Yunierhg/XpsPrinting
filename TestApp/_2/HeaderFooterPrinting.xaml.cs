using System;
using System.Windows;

namespace TestApp._2
{
    public partial class HeaderFooterPrinting : Window
    {
        private int _count = 1;
        
        public HeaderFooterPrinting()
        {
            InitializeComponent();
            UpdateTextBox();
        }

        private void UpdateTextBox()
        {
            txtCount.Text = _count.ToString();
        }

        private void IncreaseClick(object sender, RoutedEventArgs e)
        {
            _count++;
            UpdateTextBox();
        }

        private void DecreaseClick(object sender, RoutedEventArgs e)
        {
            if (_count <= 1) return;
            _count--;
            UpdateTextBox();
        }

        private void GoClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
