using System.Windows.Controls;

namespace XpsPrinting.Formatting.Utils
{
    public static class Extensions
    {
        public static void AssignFont(this TextBlock block, FontProperties font)
        {
            block.FontSize = font.FontSize;
            block.FontFamily = font.FontFamily;
            block.FontStyle = font.FontStyle;
            block.FontWeight = font.FontWeight;
        }
    }
}