using System.Windows;
using System.Windows.Media;

namespace XpsPrinting.Paging
{
    public interface IDynamicPaginator
    {
        Visual GetNextPortion(Size availableSpace);
    }
}