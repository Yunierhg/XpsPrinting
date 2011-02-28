using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using XpsPrinting.Formatting;

namespace XpsPrinting.Paging
{
    public class TemplatingPaginator : DocumentPaginator
    {
        private readonly IDocumentFormatter _main;
        private readonly IBlankPageSource _blankPageSource;
        private int _pageCount;
        
        private DocumentPage _cachedPage;
        private int _cachedPageNumber = -1;

        public TemplatingPaginator(IDocumentFormatter main, IBlankPageSource blankPageSource)
        {
            if (main == null) throw new ArgumentNullException("main");
            if (blankPageSource == null) throw new ArgumentNullException("blankPageSource");

            _main = main;
            _blankPageSource = blankPageSource;

            GetNextPageToCache();
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            if (pageNumber != _cachedPageNumber)
                throw new ArgumentOutOfRangeException("pageNumber", "Should get pages one by one");

            var result = _cachedPage;

            GetNextPageToCache();

            return result;
        }

        private void GetNextPageToCache()
        {
            int retrievingPageNumber = _cachedPageNumber + 1;
            var blankPage = _blankPageSource.CreateBlankPage(retrievingPageNumber);

            var contentVisual = _main.GetNextPortion(blankPage.DataContentBox.Size);
            if (contentVisual == null)
            {
                _pageCount = retrievingPageNumber;
                _cachedPage = DocumentPage.Missing;
                return;
            }

            var newPage = new ContainerVisual();
            newPage.Children.Add(blankPage.PageVisual);
            var transformedContentVisual = TransformVisualToBox(contentVisual, blankPage.DataContentBox);
            newPage.Children.Add(transformedContentVisual);

            _cachedPage = new DocumentPage(newPage, blankPage.PageSize, Rect.Empty, blankPage.ContentBox);
            _cachedPageNumber = retrievingPageNumber;
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

        public override bool IsPageCountValid
        {
            get { return _cachedPage == DocumentPage.Missing; }
        }

        public override int PageCount
        {
            get { return _pageCount; }
        }

        public override Size PageSize
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override IDocumentPaginatorSource Source
        {
            get { return null; }
        }
    }
}