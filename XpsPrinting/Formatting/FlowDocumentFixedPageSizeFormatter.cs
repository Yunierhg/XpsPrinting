using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace XpsPrinting.Formatting
{
    public abstract class FlowDocumentFixedPageSizeFormatter : IDocumentFormatter
    {
        private DocumentPaginator _docPaginator;
        private bool _pageSizeAdjusted;
        private Size _documentPageSize;
        private int _nextPageToRetrieveNumber;

        protected abstract FlowDocument GetDocument(Size pageSize);
        
        public Visual GetNextPortion(Size availableSpace)
        {
            if (!_pageSizeAdjusted)
            {
                AdjustPageSize(availableSpace);
            }
            else
            {
                if (_documentPageSize.Height > availableSpace.Height || _documentPageSize.Width > availableSpace.Width)
                    throw new ArgumentOutOfRangeException("availableSpace", "FlowDocumentFixedPageSizeFormatter supports only page size of underlying FlowDocument or larger one.");
            }

            var page = _docPaginator.GetPage(_nextPageToRetrieveNumber++);
            return page.Visual;
        }

        private void AdjustPageSize(Size pageSize)
        {
            var flowDocument = GetDocument(pageSize);
            _docPaginator = ((IDocumentPaginatorSource) flowDocument).DocumentPaginator;
            _documentPageSize = pageSize;
            _pageSizeAdjusted = true;
        }
    }
}