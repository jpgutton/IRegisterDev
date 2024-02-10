using DashBoardDev.DAL;
using DashBoardDev.Models;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardDev.Services
{
    public class NPOATRepository : INPOATRepository
    {
        private OrganizationContext _context;

        public CloudTable tableUserRights;
        public CloudTable tableNPORights;
        public CloudTable tableNPOCustomfields;

        public CloudStorageAccount tableStorageAccount;

        public CloudTableClient tables;

        private string _tableConnection;

        private const string DateFormat = "yyyyMMdd ; HH:mm:ss:fffffff";


        public NPOATRepository(OrganizationContext context, IOptions<AzureTablesOptions> tableConnection)
        {
            _context = context;
            _tableConnection = tableConnection.Value.AzTableConnection;

            tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);

            tables = tableStorageAccount.CreateCloudTableClient();

            tableUserRights = tables.GetTableReference("UserRights");
            tableNPORights = tables.GetTableReference("NPORights");
        }


        /// <summary>
        /// Check if user has rights for the NPO
        /// </summary>
        /// <param name="npoID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<bool> NPOCheckRights(string npoID, string userID)
        {
            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
            CloudTable azTableNPO = tableNPO.GetTableReference("NPORights");

            // Create a retrieve operation to verify if user has the right on the NPO
            TableOperation retrieveOperation = TableOperation.Retrieve<NPORights>(npoID, userID);

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableNPORights.ExecuteAsync(retrieveOperation);

            if(retrievedResult.Result != null)
            {
                return true;
            }
            return false;
        }






        /// <summary>
        /// Log action into Azure Table
        /// </summary>
        /// <param name="npoID"></param>
        /// <param name="actionType"></param>
        /// <param name="userID"></param>
        /// <param name="actionData"></param>
        //public async void AddLog(string npoID, string actionType, string userID, string userName, string actionData)
        //{
        //    CloudTable azTableNPO = tables.GetTableReference("NPOLogs");

        //    NPOATLog npoNewLog = new NPOATLog(npoID, actionType);
        //    npoNewLog.UserID = userID;
        //    npoNewLog.UserName = userName;
        //    npoNewLog.ActionData = actionData;

        //    // Create the TableOperation that inserts the customer entity.
        //    TableOperation insertOperation = TableOperation.Insert(npoNewLog);

        //    // Execute the insert operation.
        //    await azTableNPO.ExecuteAsync(insertOperation);
        //}




    }
}
