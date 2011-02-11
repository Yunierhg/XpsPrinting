using System.Windows;
using System.Windows.Media;

namespace XpsPrinting.Paging
{
    public interface IBlankPage
    {
        Rect ContentBox { get; }
        Rect BleedBox { get; }
        Rect DataContentBox { get; }
        Size PageSize { get; set; }
        Visual PageVisual { get; }
    }
}