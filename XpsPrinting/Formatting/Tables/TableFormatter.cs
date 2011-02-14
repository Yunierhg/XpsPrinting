using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using XpsPrinting.Formatting.Utils;

namespace XpsPrinting.Formatting.Tables
{
    public class TableFormatter
    {
        public FontProperties HeaderFont { get; private set; }
        public FontProperties TableCellFont { get; private set; }

        public TableFormatter()
        {
            HeaderFont = new FontProperties {FontWeight = FontWeights.Bold};
            TableCellFont = new FontProperties();
        }

        public Table FormatData(DataView data, IEnumerable<PrintColumnInfo> columnsInfo, double width)
        {
            var contentTable = new Table();

            //find the longes columns values from the data
            Dictionary<PrintColumnInfo, string> columnsMaxValues = GetCellMaxLengthRow(data, columnsInfo);
            //Compute columns width in UI Grid first and than use them in FlowDocument Table
            /* Unfortunately Table element from FlowDocument does not support auto column widthes
             * and Grid elemend could not be printed on multiple pages.
             * That's why I used "doble rendering" trick. First render text in UI and store columns
             * width and then use determined columns width in FlowDocument Table
             */
            //Creating fake grid to determing columns width
            var fakeGrid = new Grid();
            // creting real data width on print page
            fakeGrid.Width = width;
            //setting width of the UI Grid columns
            foreach (PrintColumnInfo colInfo in columnsInfo)
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
                tbHeader.AssignFont(HeaderFont);
                tbHeader.Text = columnInfo.Key.ColumnHeader;
                fakeGrid.Children.Add(tbHeader);
                Grid.SetColumn(tbHeader, counter);
                Grid.SetRow(tbHeader, 0);
                //add data
                var tbContent = new TextBlock();
                tbContent.Padding = new Thickness(2);
                tbContent.AssignFont(TableCellFont);
                tbContent.Text = columnInfo.Value;
                fakeGrid.Children.Add(tbContent);
                Grid.SetColumn(tbContent, counter);
                Grid.SetRow(tbContent, 1);

                counter++;
            }

            // Fake rendering. It causes grid to calculate its children size 
            // without doing actual rendering
            fakeGrid.Measure(new Size(width, double.MaxValue));
            fakeGrid.Arrange(new Rect(new Size(width, double.MaxValue)));

            //getting columns width
            var columnWidthes = new double[columnsMaxValues.Count];
            for (int i = 0; i < columnWidthes.Length; i++)
            {
                columnWidthes[i] = fakeGrid.ColumnDefinitions[i].ActualWidth;
            }

            int columnCount = columnsInfo.Count();
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
            foreach (PrintColumnInfo column in columnsInfo)
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
            foreach (DataRowView dataRowView in data)
            {
                currentRow = new TableRow();
                currentRow.Background = ++colorCount % 2 == 0 ? Brushes.LightGray : Brushes.White;
                foreach (PrintColumnInfo columnInfo in columnsInfo)
                {
                    string columnValue = dataRowView[columnInfo.ColumnName] != null ?
                                                                                        dataRowView[columnInfo.ColumnName].ToString() : string.Empty;
                    var tc = new TableCell(new Paragraph(new Run(columnValue)));
                    currentRow.Cells.Add(tc);
                }
                contentTable.RowGroups[0].Rows.Add(currentRow);
            }

            return contentTable;
        }

        private static Dictionary<PrintColumnInfo, string> GetCellMaxLengthRow(DataView data, IEnumerable<PrintColumnInfo> columnsInfo)
        {
            var columnsMaxValues = new Dictionary<PrintColumnInfo, string>();
            IEnumerable<DataRow> allRows = data.Table.AsEnumerable();
            foreach (PrintColumnInfo columnInfo in columnsInfo)
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
    }
}