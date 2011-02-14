namespace XpsPrinting.Formatting.Tables
{
    public class PrintColumnInfo
    {
        private readonly string _columnName;
        private readonly string _columnHeader;
        private readonly PrintLength _columnWidth;

        //If no column header was specified column name is assigned to column header
        public PrintColumnInfo(string columnName)
            : this(columnName, columnName)
        {
        }

        //if no PrintLength specified column will be sized propertionaly to avalable space
        public PrintColumnInfo(string columnName, string columnHeader)
            : this(columnName, columnHeader, new PrintLength(1, PrintUnitType.Star))
        {
        }

        public PrintColumnInfo(string columnName, string columnHeader, PrintLength columnWidth)
        {
            _columnName = columnName;
            _columnHeader = columnHeader;
            _columnWidth = columnWidth;
        }

        public string ColumnName
        {
            get { return _columnName; }
        }

        public string ColumnHeader
        {
            get { return _columnHeader; }
        }

        public PrintLength ColumnWidth
        {
            get { return _columnWidth; }
        }
    }
}