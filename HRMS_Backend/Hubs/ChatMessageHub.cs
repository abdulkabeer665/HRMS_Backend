using Microsoft.AspNetCore.SignalR;
using System.Data;

namespace HRMS_Backend.Hubs
{
    public class ChatMessageHub : Hub
    {
        public async Task SendChatMessages123(DataTable dataTable)
        {
            var dataList = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                var rowDict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    string columnName = column.ColumnName;
                    string modifiedColumnName = char.ToLower(columnName[0]) + columnName.Substring(1);
                    rowDict[modifiedColumnName] = row[column];
                }
                dataList.Add(rowDict);
            }
            await Clients.All.SendAsync("ReceiveChatMessages123", dataList);
        }
    }
}
