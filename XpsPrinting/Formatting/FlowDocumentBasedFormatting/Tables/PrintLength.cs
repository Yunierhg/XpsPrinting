namespace XpsPrinting.Formatting.FlowDocumentBasedFormatting.Tables
{
    public struct PrintLength
    {
        private static readonly PrintLength _auto;

        private readonly double _value;
        private readonly PrintUnitType _unitType;

        static PrintLength()
        {
            _auto = new PrintLength(1, PrintUnitType.Auto);
        }

        public PrintLength(double pixels)
            : this(pixels, PrintUnitType.Pixel)
        {
        }

        public PrintLength(double value, PrintUnitType unitType)
        {
            _value = value;
            _unitType = unitType;
        }

        public static PrintLength Auto
        {
            get { return _auto; }
        }

        public PrintUnitType PrintUnitType
        {
            get { return _unitType; }
        }

        public double Value
        {
            get { return _value; }
        }

        public bool IsStar
        {
            get { return (_unitType == PrintUnitType.Star); }
        }

        public bool IsAbsolute
        {
            get { return (_unitType == PrintUnitType.Pixel); }
        }

        public bool IsAuto
        {
            get { return (_unitType == PrintUnitType.Auto); }
        }

        public override bool Equals(object obj)
        {
            if (obj is PrintLength)
            {
                var length = (PrintLength) obj;
                return (this == length);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ((int) _value + (int) _unitType);
        }

        public static bool operator ==(PrintLength length1, PrintLength length2)
        {
            return ((length1.PrintUnitType == length2.PrintUnitType) && (length1.Value == length2.Value));
        }

        public static bool operator !=(PrintLength length1, PrintLength length2)
        {
            if (length1.PrintUnitType == length2.PrintUnitType)
            {
                return (length1.Value != length2.Value);
            }
            return true;
        }
    }

    public enum PrintUnitType
    {
        Auto = 0,
        Pixel = 1,
        Star = 2
    }
}