using System;
using System.Windows;
using System.Windows.Media;

namespace XpsPrinting.Paging
{
    public class Page : IPage
    {
        private readonly Size _pageSize;
        private readonly Rect _contentBox;
        private readonly Visual _visual;

        public Page(IBlankPage blankPage, Visual contentVisual)
        {
            if (blankPage == null) throw new ArgumentNullException("blankPage");
            if (contentVisual == null) throw new ArgumentNullException("contentVisual");

            _pageSize = blankPage.PageSize;
            _contentBox = blankPage.ContentBox;

            _visual = CreatePageVisual(blankPage, contentVisual);
        }

        private ContainerVisual CreatePageVisual(IBlankPage blankPage, Visual contentVisual)
        {
            var newPage = new ContainerVisual();
            newPage.Children.Add(blankPage.PageVisual);
            newPage.Children.Add(TransformVisualToBox(contentVisual, blankPage.DataContentBox));
            return newPage;
        }

        private static Visual TransformVisualToBox(Visual contentVisual, Rect dataContentBox)
        {
            var visual = new ContainerVisual
            {
                Clip = new RectangleGeometry(new Rect(dataContentBox.Size))
            };
            visual.Children.Add(contentVisual);
            visual.Transform = new TranslateTransform(dataContentBox.Left, dataContentBox.Top);
            return visual;
        }

        public Size PageSize
        {
            get { return _pageSize; }
        }

        public Rect ContentBox
        {
            get { return _contentBox; }
        }

        public Visual Visual
        {
            get { return _visual; }
        }
    }
}