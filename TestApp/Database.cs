using System;
using System.Data;
using System.Threading;
using System.Windows.Threading;

namespace TestApp
{
    internal static class Database
    {
        private const int StartDataCount = 50;
        private static readonly DataTable DtLogs = new DataTable("ActivityLog");
        private static Dispatcher _currentDispatcher;

        public static DataTable GetLogData()
        {
            _currentDispatcher = Dispatcher.CurrentDispatcher;
            DtLogs.Columns.Add("Order");
            DtLogs.Columns.Add("User");
            DtLogs.Columns.Add("Object");
            DtLogs.Columns.Add("Message");
            DtLogs.Columns.Add("OldValue");
            DtLogs.Columns.Add("NewValue");
            DtLogs.Columns.Add("Date");

            for (int i = 0; i < StartDataCount; i++)
            {
                DataRow currentRow = DtLogs.NewRow();
                currentRow["Order"] = i;
                currentRow["User"] = "UserName";
                currentRow["Object"] = i%3 == 0 ? "Workspace" : "Project";
                currentRow["Message"] = i%5 == 0 ? "Short Message" :
                                                                       "Very very long message to ensure that Grid and Printing works correctly and word wraps works too";
                currentRow["OldValue"] = currentRow["NewValue"] = i%10 == 0 ? "Value" : string.Empty;
                currentRow["Date"] = DateTime.Now;
                DtLogs.Rows.Add(currentRow);
            }

            //Launch Updates
            var updateThread = new Thread(UpdateData);
            updateThread.IsBackground = true;
            updateThread.Start();
            return DtLogs;
        }

        private static void UpdateData()
        {
            int counter = StartDataCount;
            while (true)
            {
                var newLog = new DataTable("ActivityLog");
                newLog.Columns.Add("Order");
                newLog.Columns.Add("User");
                newLog.Columns.Add("Object");
                newLog.Columns.Add("Message");
                newLog.Columns.Add("OldValue");
                newLog.Columns.Add("NewValue");
                newLog.Columns.Add("Date");
                Thread.Sleep(TimeSpan.FromSeconds(3));
                DataRow newRow = newLog.NewRow();
                newRow["Order"] = ++counter;
                newRow["User"] = "Updated User";
                newRow["Object"] = "Update";
                newRow["Message"] = counter%5 == 0 ? "Update Message" :
                                                                          "Update long message to ensure that Grid and Printing works correctly and word wraps works too";
                newRow["OldValue"] = newRow["NewValue"] = counter%10 == 0 ? "Update" : string.Empty;
                newRow["Date"] = DateTime.Now;
                newLog.Rows.Add(newRow);
                _currentDispatcher.BeginInvoke(DispatcherPriority.DataBind, (SendOrPostCallback) delegate { DtLogs.Merge(newLog); }, null);
            }
        }
    }
}