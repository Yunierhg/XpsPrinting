using System.Windows;
using XpsPrinting.Paging;

namespace TestApp._2
{
    public partial class SimpleBlankPage : BlankPageBase
    {
        public SimpleBlankPage()
        {
            InitializeComponent();
        }

        public override Rect DataContentBox
        {
            get
            {
                Measure(PageSize);
                Arrange(ContentBox);
                var transformToAncestor = contentPlaceholder.TransformToAncestor(this);
                var topLeft = new Point(contentPlaceholder.Padding.Left, contentPlaceholder.Padding.Top);
                var bottomRight = new Point(contentPlaceholder.ActualWidth - contentPlaceholder.Padding.Right, contentPlaceholder.ActualHeight - contentPlaceholder.Padding.Bottom);
                return new Rect(transformToAncestor.Transform(topLeft), transformToAncestor.Transform(bottomRight));
            }
        }

        public string Header
        {
            set { tbHeader.Text = value; }
        }

        public string Footer
        {
            set { tbFooter.Text = value; }
        }
    }
}