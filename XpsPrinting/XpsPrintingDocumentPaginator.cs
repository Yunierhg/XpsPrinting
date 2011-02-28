using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using XpsPrinting.Documents;

namespace XpsPrinting
{
    public class XpsPrintingDocumentPaginator : DocumentPaginator
    {
        private readonly IDocument _document;

        private IEnumerator<IPage> _currentEnumerator;
        private int _currentPageNumber = -1;
        private bool _isPageCountValid;
        private int _pageCount;

        public XpsPrintingDocumentPaginator(IDocument document)
        {
            if (document == null) throw new ArgumentNullException("document");

            _document = document;

            // if document is empty - IsPageCountValid and PageCount should become 0 and true instantly. So fetch first page eagerly
            _currentEnumerator = _document.GetEnumerator();
            GetNextPage();
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            if (pageNumber < 0 || _isPageCountValid && pageNumber >= _pageCount)
                return DocumentPage.Missing;

            EnsureEnumeratorIsCorrect(pageNumber);

            while (pageNumber > _currentPageNumber)
            {
                if (!GetNextPage())
                    return DocumentPage.Missing;
            }

            var page = _currentEnumerator.Current;
            if (page == null) throw new InvalidReturnValue("IDocument's enumerator returned null as Current");

            // DocumentPaginator clients inside .NET Framework are implicitly assuming that while IsPageCountValid returns false
            // and GetPage(x) was already called successfully - GetPage(x+1) should return real page, not DocumentPage.Missing.
            // So, when getting page with number x if we don't know already correct page count - we should check that (x+1) page exists or set 
            // also PageCount and IsPageCountValid appropriately.
            if (!_isPageCountValid)
                GetNextPage();

            return CreateDocumentPage(page);
        }

        // virtual is here for the testability
        protected virtual DocumentPage CreateDocumentPage(IPage page)
        {
            return new DocumentPage(page.Visual, page.PageSize, Rect.Empty, page.ContentBox);
        }

        private bool GetNextPage()
        {
            if (!_currentEnumerator.MoveNext())
            {
                _currentEnumerator.Dispose();
                _currentEnumerator = null;
                _pageCount = _currentPageNumber + 1;
                _isPageCountValid = true;
                _currentPageNumber = -1;
                return false;
            }
            _currentPageNumber++;
            return true;
        }

        /// <summary>
        /// Ensures that _currentPageNumber is less or equal to pageNumber param, thus we could obtain requested page through
        /// _currentEnumerator. Otherwise it recreates _currentEnumerator and resets _currentPageNumber.
        /// </summary>
        private void EnsureEnumeratorIsCorrect(int pageNumber)
        {
            if (_currentEnumerator == null || _currentPageNumber > pageNumber)
            {
                if (_currentEnumerator != null) _currentEnumerator.Dispose();
                _currentEnumerator = _document.GetEnumerator();
                _currentPageNumber = -1;
            }
        }

        public override bool IsPageCountValid
        {
            get { return _isPageCountValid; }
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