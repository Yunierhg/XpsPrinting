using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace XpsPrinting.Formatting
{
    public class LogPrintDocumentCreator
    {
        private IList<PrintColumnInfo> _columnsInfo;
        private DataView _data;

        public LogPrintDocumentCreator(DataView data)
        {
            _data = data;
            //if columns info is null than all column will have headers as columns' names
            // and all colums will have equal width 
            _columnsInfo = new List<PrintColumnInfo>(data.Table.Columns.Count);
            for (int i = 0; i < data.Table.Columns.Count; i++)
            {
                _columnsInfo.Add(new PrintColumnInfo(data.Table.Columns[i].ColumnName));
            }
        }

        public LogPrintDocumentCreator(DataView data, IList<PrintColumnInfo> columnsInfo)
        {
            _data = data;
            _columnsInfo = columnsInfo;
        }

        public FlowDocument GetDocument(Size pageSize, Thickness margins)
        {
            //Load Document from the template file
            FlowDocument targetDocument = LoadDocumentTemplate();
            //find section named PlaceHolder there
            var placeHolder = targetDocument.FindName("PlaceHolder") as Section;
            if (placeHolder == null)
                return null;
            //Create Table as logs will be displayed as tabular data
            var contentTable = new Table();
            placeHolder.Blocks.Add(contentTable);

            //Get Fontsize settings from the placeholder
            FontFamily fontFamily = placeHolder.FontFamily;
            double fontSize = placeHolder.FontSize;
            FontWeight fontWeight = placeHolder.FontWeight;
            FontStyle fontStyle = placeHolder.FontStyle;

            //find the longes columns values from the data
            Dictionary<PrintColumnInfo, string> columnsMaxValues = GetCellMaxLengthRow();
            //Compute columns width in UI Grid first and than use them in FlowDocument Table
            /* Unfortunately Table element from FlowDocument does not support auto column widthes
             * and Grid elemend could not be printed on multiple pages.
             * That's why I used "doble rendering" trick. First render text in UI and store columns
             * width and then use determined columns width in FlowDocument Table
             */
            //Creating fake grid to determing columns width
            var fakeGrid = new Grid();
            // creting real data width on print page
            double width = pageSize.Width - (margins.Left + margins.Right) - (_columnsInfo.Count*contentTable.CellSpacing*2);
            fakeGrid.Width = width;
            //setting width of the UI Grid columns
            foreach (PrintColumnInfo colInfo in _columnsInfo)
            {
                var colDef = new ColumnDefinition();
                colDef.Width = PrintLengthToGridLengthConverter.Convert(colInfo.ColumnWidth);
                fakeGrid.ColumnDefinitions.Add(colDef);
            }
            // adding header row and content row to the grid
            // as content width could be smaller than header width
            var headerRow = new RowDefinition();
            headerRow.Height = GridLength.Auto;
            fakeGrid.RowDefinitions.Add(headerRow);
            var contentRow = new RowDefinition();
            contentRow.Height = GridLength.Auto;
            fakeGrid.RowDefinitions.Add(contentRow);

            //adding content and header to the UI grid
            int counter = 0;
            foreach (KeyValuePair<PrintColumnInfo, string> columnInfo in columnsMaxValues)
            {
                //add header
                var tbHeader = new TextBlock();
                tbHeader.Padding = new Thickness(2);
                tbHeader.FontSize = fontSize;
                tbHeader.FontFamily = fontFamily;
                tbHeader.FontStyle = fontStyle;
                tbHeader.FontWeight = FontWeights.Bold;
                tbHeader.Text = columnInfo.Key.ColumnHeader;
                fakeGrid.Children.Add(tbHeader);
                Grid.SetColumn(tbHeader, counter);
                Grid.SetRow(tbHeader, 0);
                //add data
                var tbContent = new TextBlock();
                tbContent.Padding = new Thickness(2);
                tbContent.FontSize = fontSize;
                tbContent.FontFamily = fontFamily;
                tbContent.FontStyle = fontStyle;
                tbContent.FontWeight = fontWeight;
                tbContent.Text = columnInfo.Value;
                fakeGrid.Children.Add(tbContent);
                Grid.SetColumn(tbContent, counter);
                Grid.SetRow(tbContent, 1);

                counter++;
            }

            // Fake rendering. It causes grid to calculate its children size 
            // without doing actual rendering
            fakeGrid.Measure(new Size(width, pageSize.Height));
            fakeGrid.Arrange(new Rect(new Size(width, pageSize.Height)));

            //getting columns width
            var columnWidthes = new double[columnsMaxValues.Count];
            for (int i = 0; i < columnWidthes.Length; i++)
            {
                columnWidthes[i] = fakeGrid.ColumnDefinitions[i].ActualWidth;
            }

            int columnCount = _columnsInfo.Count;
            // adding column and setting their width
            for (int i = 0; i < columnCount; i++)
            {
                var tableColumn = new TableColumn();
                tableColumn.Width = new GridLength(columnWidthes[i]);
                contentTable.Columns.Add(tableColumn);
            }

            //adding headers for column
            contentTable.RowGroups.Add(new TableRowGroup());
            contentTable.RowGroups[0].Rows.Add(new TableRow());
            TableRow currentRow = contentTable.RowGroups[0].Rows[0];
            foreach (PrintColumnInfo column in _columnsInfo)
            {
                currentRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(column.ColumnHeader)))));
            }
            //adding Blue line
            contentTable.RowGroups[0].Rows.Add(new TableRow());
            currentRow = contentTable.RowGroups[0].Rows[1];
            var lineCell = new TableCell();
            lineCell.ColumnSpan = columnCount;
            lineCell.BorderThickness = new Thickness(0, 2, 0, 0);
            lineCell.BorderBrush = Brushes.Blue;
            currentRow.Cells.Add(lineCell);
            int colorCount = 0;

            // adding data to the table
            foreach (DataRowView dataRowView in _data)
            {
                currentRow = new TableRow();
                currentRow.Background = ++colorCount%2 == 0 ? Brushes.LightGray : Brushes.White;
                foreach (PrintColumnInfo columnInfo in _columnsInfo)
                {
                    string columnValue = dataRowView[columnInfo.ColumnName] != null ?
                                                                                        dataRowView[columnInfo.ColumnName].ToString() : string.Empty;
                    var tc = new TableCell(new Paragraph(new Run(columnValue)));
                    currentRow.Cells.Add(tc);
                }
                contentTable.RowGroups[0].Rows.Add(currentRow);
            }

            return targetDocument;
        }

        public IList<PrintColumnInfo> PrintColumnsInfo
        {
            get { return _columnsInfo; }
            set { _columnsInfo = value; }
        }

        /// <summary>
        /// Finds the longest values from the log DataView and returns them
        /// </summary>
        /// <returns></returns>
        private Dictionary<PrintColumnInfo, string> GetCellMaxLengthRow()
        {
            var columnsMaxValues = new Dictionary<PrintColumnInfo, string>();
            IEnumerable<DataRow> allRows = _data.Table.AsEnumerable();
            foreach (PrintColumnInfo columnInfo in _columnsInfo)
            {
                //get the max value for particular column
                IEnumerable<DataRow> maxLengthRow = from row in allRows
                                                    where row[columnInfo.ColumnName] != null
                                                    orderby row[columnInfo.ColumnName].ToString().Length descending
                                                    select row;

                //if colum has null values than no records will be returned
                IEnumerator<DataRow> rowEnumerator = maxLengthRow.GetEnumerator();
                string value = rowEnumerator.MoveNext() ? rowEnumerator.Current[columnInfo.ColumnName].ToString() : string.Empty;
                columnsMaxValues.Add(columnInfo, value);
            }
            return columnsMaxValues;
        }

        protected FlowDocument LoadDocumentTemplate()
        {
            return new ActivityLogDocumentTemplate();
        }
    }
}