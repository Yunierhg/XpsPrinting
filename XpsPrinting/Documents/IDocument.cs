using System.Collections.Generic;
using XpsPrinting.Paging;

namespace XpsPrinting.Documents
{
    public interface IDocument
    {
        IEnumerable<IPage> GetPages();
    }
}