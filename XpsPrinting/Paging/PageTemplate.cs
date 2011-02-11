using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XpsPrinting.Paging
{
    public abstract class PageTemplate : UserControl, IBlankPage
    {
        public abstract Rect DataContentBox { get; }

        public Rect ContentBox
        {
            get { return new Rect(Margin.Left + ActualWidth, Margin.Top + ActualHeight, ActualWidth, ActualHeight); }
        }

        public Rect BleedBox
        {
            get { return Rect.Empty; }
        }

        public Size PageSize
        {
            get { return new Size(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public Visual PageVisual
        {
            get { return this; }
        }
    }
}