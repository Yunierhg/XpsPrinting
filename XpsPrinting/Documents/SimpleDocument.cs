using System;
using System.Collections.Generic;
using XpsPrinting.Formatting;
using XpsPrinting.Paging;

namespace XpsPrinting.Documents
{
    public class SimpleDocument : IDocument
    {
        public SimpleDocument(IBlankPageSource blankPageSource, IDataFormatter dataFormatter)
        {
            
        }

        public IEnumerable<IPage> GetPages()
        {
            throw new NotImplementedException();
        }
    }
}