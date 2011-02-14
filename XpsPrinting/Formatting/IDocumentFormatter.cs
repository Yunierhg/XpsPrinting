using System.Windows;
using System.Windows.Media;

namespace XpsPrinting.Formatting
{
    public interface IDocumentFormatter
    {
        Visual GetNextPortion(Size availableSpace);
    }
}