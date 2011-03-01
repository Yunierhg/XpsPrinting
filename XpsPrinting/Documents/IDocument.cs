using System.Collections.Generic;

namespace XpsPrinting.Documents
{
    public interface IDocument
    {
        IEnumerable<IPage> GetPages();
    }
}