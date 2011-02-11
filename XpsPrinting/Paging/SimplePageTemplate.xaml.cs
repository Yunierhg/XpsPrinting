using System.Windows;

namespace XpsPrinting.Paging
{
    public partial class SimplePageTemplate : PageTemplate
    {
        public SimplePageTemplate()
        {
            InitializeComponent();
        }

        public override Rect DataContentBox
        {
            get
            {
                Measure(PageSize);
                Arrange(new Rect(0, 0, Width, Height));
                var transformToAncestor = contentPlaceholder.TransformToAncestor(this);
                var topLeft = new Point(contentPlaceholder.Padding.Left, contentPlaceholder.Padding.Top);
                var bottomRight = new Point(contentPlaceholder.ActualWidth - contentPlaceholder.Padding.Right, contentPlaceholder.ActualHeight - contentPlaceholder.Padding.Bottom);
                return new Rect(transformToAncestor.Transform(topLeft), transformToAncestor.Transform(bottomRight));
            }
        }

        public string PageNumber
        {
            set { pageNumber.Text = value; }
        }
    }
}