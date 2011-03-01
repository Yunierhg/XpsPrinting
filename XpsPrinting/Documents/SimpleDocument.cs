using System;
using System.Collections.Generic;
using XpsPrinting.Formatting;
using XpsPrinting.Paging;

namespace XpsPrinting.Documents
{
    public class SimpleDocument : IDocument
    {
        private readonly IBlankPageSource _blankPageSource;
        private readonly IDataFormatter _dataFormatter;

        public SimpleDocument(IDataFormatter dataFormatter)
            : this(new RelayedBlankPageSource(_ => new BlankPageBase()), dataFormatter)
        {
        }

        public SimpleDocument(IBlankPageSource blankPageSource, IDataFormatter dataFormatter)
        {
            if (blankPageSource == null) throw new ArgumentNullException("blankPageSource");
            if (dataFormatter == null) throw new ArgumentNullException("dataFormatter");

            _blankPageSource = blankPageSource;
            _dataFormatter = dataFormatter;
        }

        public IEnumerable<IPage> GetPages()
        {
            for (int pageNumber = 0; ; pageNumber++)
            {
                IBlankPage blankPage = _blankPageSource.CreateBlankPage(pageNumber);
                var pageVisual = _dataFormatter.GetNextPortion(blankPage.DataContentBox.Size);
                if (pageVisual == null)
                    yield break;

                yield return new Page(blankPage, pageVisual);
            }
        }
    }
}