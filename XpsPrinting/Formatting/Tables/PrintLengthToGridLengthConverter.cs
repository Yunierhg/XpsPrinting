using System.Windows;

namespace XpsPrinting.Formatting.Tables
{
    internal static class PrintLengthToGridLengthConverter
    {
        public static GridLength Convert(PrintLength printLength)
        {
            if (printLength.PrintUnitType == PrintUnitType.Star)
            {
                return new GridLength(printLength.Value, GridUnitType.Star);
            }
            if (printLength.PrintUnitType == PrintUnitType.Pixel)
            {
                return new GridLength(printLength.Value, GridUnitType.Pixel);
            }
            return GridLength.Auto;
        }
    }
}