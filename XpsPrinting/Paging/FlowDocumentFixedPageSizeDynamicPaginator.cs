using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace XpsPrinting.Paging
{
    public class FlowDocumentFixedPageSizeDynamicPaginator : IDynamicPaginator
    {
        private readonly Func<Size, FlowDocument> _flowDocumentSource;

        private DocumentPaginator _docPaginator;
        private bool _pageSizeAdjusted;
        private Size _documentPageSize;
        private int _nextPageToRetrieveNumber;

        public FlowDocumentFixedPageSizeDynamicPaginator(Func<Size, FlowDocument> flowDocumentSource)
        {
            if (flowDocumentSource == null) throw new ArgumentNullException("flowDocumentSource");

            _flowDocumentSource = flowDocumentSource;
        }

        public Visual GetNextPortion(Size availableSpace)
        {
            if (!_pageSizeAdjusted)
            {
                AdjustPageSize(availableSpace);
            }
            else
            {
                if (_documentPageSize.Height > availableSpace.Height || _documentPageSize.Width > availableSpace.Width)
                    throw new ArgumentOutOfRangeException("availableSpace", "FlowDocumentFixedPageSizeDynamicPaginator supports only page size of underlying FlowDocument or larger one.");
            }

            var page = _docPaginator.GetPage(_nextPageToRetrieveNumber++);
            return page.Visual;
        }

        private void AdjustPageSize(Size pageSize)
        {
            var flowDocument = _flowDocumentSource(pageSize);
            _docPaginator = ((IDocumentPaginatorSource) flowDocument).DocumentPaginator;
            _documentPageSize = pageSize;
            _pageSizeAdjusted = true;
        }
    }
}