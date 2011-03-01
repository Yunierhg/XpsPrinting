using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace XpsPrinting.Formatting
{
    /// <summary>
    /// Formatter for data represented with DocumentPaginator.
    /// </summary>
    /// <remarks>
    /// Its main limitation is impossibility to reflow data when there's available less space that needed by the page 
    /// returned from DocumentPaginator. So, the only real usage is when we know that pages are fixed-size and selecting
    /// page template that will guarantee at least as much space as page size consumes.
    /// 
    /// So, when first chunk of formatted data is requested, we treat availableSpace as that fixed page size.
    /// </remarks>
    public abstract class DocumentPaginatorDataFormatter : IDataFormatter
    {
        private DocumentPaginator _docPaginator;
        private int _nextPageToRetrieveNumber;

        protected abstract DocumentPaginator GetDocumentPaginator(Size maxPageSize);
        
        public Visual GetNextPortion(Size availableSpace)
        {
            if (_docPaginator == null)
                _docPaginator = GetDocumentPaginator(availableSpace);
            
            var page = _docPaginator.GetPage(_nextPageToRetrieveNumber++);

            if (page == DocumentPage.Missing)
                return null;

            var pageSize = page.Size;

            if (pageSize.Height > availableSpace.Height || pageSize.Width > availableSpace.Width)
                    throw new ArgumentOutOfRangeException("availableSpace");

            return page.Visual;
        }

        public void ResetPosition()
        {
            _nextPageToRetrieveNumber = 0;
        }
    }
}