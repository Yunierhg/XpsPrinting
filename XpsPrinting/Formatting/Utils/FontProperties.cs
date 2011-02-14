using System.Windows;
using System.Windows.Media;

namespace XpsPrinting.Formatting.Utils
{
    public class FontProperties
    {
        public FontProperties()
        {
            FontSize = 12;
            FontFamily = new FontFamily("Arial");
            FontStyle = FontStyles.Normal;
            FontWeight = FontWeights.Normal;
        }

        public double FontSize { get; set; }
        public FontFamily FontFamily { get; set; }
        public FontStyle FontStyle { get; set; }
        public FontWeight FontWeight { get; set; }
    }
}