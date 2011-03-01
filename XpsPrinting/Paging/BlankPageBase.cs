using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XpsPrinting.Paging
{
    /// <summary>
    /// It is the basic implementation of IBlankPage based on UserControl. It provides just blank page with margins and same DataContentBox
    /// and ContentBox (so no service information is anticipated).
    /// 
    /// It is designed to be inherited by other templates to create useful templates with service info like customized headers/footers etc.
    /// </summary>
    public class BlankPageBase : UserControl, IBlankPage
    {
        public virtual Rect DataContentBox
        {
            get { return ContentBox; }
        }

        public virtual Rect ContentBox
        {
            get
            {
                var width = PageSize.Width - Margin.Left - Margin.Right;
                var heigth = PageSize.Height - Margin.Top - Margin.Bottom;
                return new Rect(Margin.Left, Margin.Top, width, heigth);
            }
        }

        public virtual Size PageSize
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