using System.Windows;
using System.Windows.Media;

namespace XpsPrinting.Paging
{
    /// <summary>
    /// Represents ready-to-print readonly page abstraction.
    /// </summary>
    public interface IPage
    {
        Size PageSize { get; }
        Rect ContentBox { get; }
        Visual Visual { get; }
    }
}