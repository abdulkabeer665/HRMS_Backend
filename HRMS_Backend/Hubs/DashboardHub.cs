using Microsoft.AspNetCore.SignalR;
using System;
using System.Data;
using System.Text.RegularExpressions;

namespace HRMS_Backend.Hubs
{
    public class DashboardHub : Hub
    {
        public async Task SendAssetsCount(DataTable dataTable)
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
            await Clients.All.SendAsync("ReceiveAssetsCount", dataList);
        }

        public async Task SendMaxAssetsCountOnLocation(DataTable dataTable)
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
            await Clients.All.SendAsync("ReceiveAssetsCountOnLocation", dataList);
        }

        public async Task SendLastAssetPurchaseDate(DataTable dataTable)
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
            await Clients.All.SendAsync("ReceiveLastAssetPurchaseDate", dataList);
        }

        public async Task SendTotalAssetsCost(DataTable dataTable)
        {
            var dataList = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                var rowDict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    string columnName = column.ColumnName;
                    string modifiedColumnName = char.ToLower(columnName[0]) + column.ColumnName.Substring(1);
                    rowDict[modifiedColumnName] = row[column];
                }
                dataList.Add(rowDict);
            }
            await Clients.All.SendAsync("ReceiveTotalAssetsCost", dataList);
        }

        public async Task SendHighestTotalAssetCost(DataTable dataTable)
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
            await Clients.All.SendAsync("ReceiveHighestTotalAssetCost", dataList);
        }

        public async Task SendUsersCount(DataTable dataTable)
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
            await Clients.All.SendAsync("ReceiveUsersCount", dataList);
        }

        public async Task SendAssetsPerYearExpense(DataTable dataTable)
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
            await Clients.All.SendAsync("ReceiveAssetsPerYearExpense", dataList);
        }
        public async Task JoinGroup(string astID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, astID);
            Console.WriteLine($"Connection {Context.ConnectionId} joined group {astID}");
        }

        public async Task LeaveGroup(string astID)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, astID);
            Console.WriteLine($"Connection {Context.ConnectionId} left group {astID}");
        }

        public async Task SendChatMessages(DataTable dataTable)
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
            await Clients.All.SendAsync("ReceiveChatMessages", dataList);
        }
    }
}
