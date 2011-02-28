using System.Windows;
using System.Windows.Media;

namespace XpsPrinting.Documents
{
    public interface IPage
    {
        Size PageSize { get; }
        Rect ContentBox { get; }
        Visual Visual { get; }
    }
}