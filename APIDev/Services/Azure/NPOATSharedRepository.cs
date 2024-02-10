using APIDev.DAL;
using APIDev.Models;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIDev.Services
{
    public class NPOATSharedRepository : INPOATSharedRepository
    {
        private OrganizationContext _context;

        public CloudTable tableUserRights;
        public CloudTable tableNPORights;
        public CloudTable tableNPOCustomfields;

        public CloudStorageAccount tableStorageAccount;

        public CloudTableClient tables;

        private string _tableConnection;

        private const string DateFormat = "yyyyMMdd - HH:mm:ss";


        public NPOATSharedRepository(OrganizationContext context, IOptions<AzureTablesOptions> tableConnection)
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
        /// Create a new NPO Table
        /// </summary>
        /// <param name="NPOSiteID"></param>
        public async void CreateTable(string NPOSiteID)
        {
            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
            CloudTable azTableNPO = tableNPO.GetTableReference(NPOSiteID);
            await azTableNPO.CreateAsync();

            CloudTable azTableNPOFields = tableNPO.GetTableReference(NPOSiteID + "fieldname");
            await azTableNPOFields.CreateAsync();
        }


        /// <summary>
        /// Create new Roles for the NPO
        /// </summary>
        /// <param name="NPOSiteID"></param>
        /// <param name="UserID"></param>
        public async void AddNewNPORole(string ClubID, string NPOSiteID, string UserID, string UserName)
        {
            // Create the batch operation.
            TableBatchOperation batchOperationUserRights = new TableBatchOperation();

            // Add row for roles
            NPORights npoUserRights1 = new NPORights(UserID, ClubID);
            npoUserRights1.UserName = UserName;
            npoUserRights1.AddDate = DateTime.Now.ToUniversalTime().ToString(DateFormat);

            NPORights npoUserRights2 = new NPORights(UserID, NPOSiteID);
            npoUserRights2.UserName = UserName;
            npoUserRights2.AddDate = DateTime.Now.ToUniversalTime().ToString(DateFormat);

            // Add both customer entities to the batch insert operation.
            batchOperationUserRights.Insert(npoUserRights1);
            batchOperationUserRights.Insert(npoUserRights2);

            // Execute the batch operation.
            await tableUserRights.ExecuteBatchAsync(batchOperationUserRights);

            // Create the batch operation.
            TableBatchOperation batchOperationNPORights = new TableBatchOperation();

            // Add row for roles
            NPORights npoNPORights1 = new NPORights(ClubID, UserID);
            npoNPORights1.UserName = UserName;
            npoNPORights1.AddDate = DateTime.Now.ToUniversalTime().ToString(DateFormat);

            batchOperationNPORights.Insert(npoNPORights1);
            await tableNPORights.ExecuteBatchAsync(batchOperationNPORights);

            TableBatchOperation batchOperationNPORights2 = new TableBatchOperation();

            // Add row for roles
            NPORights npoNPORights2 = new NPORights(NPOSiteID, UserID);
            npoNPORights2.UserName = UserName;
            npoNPORights2.AddDate = DateTime.Now.ToUniversalTime().ToString(DateFormat);

            // Add both customer entities to the batch insert operation.
            batchOperationNPORights2.Insert(npoNPORights2);

            // Execute the batch operation.
            await tableNPORights.ExecuteBatchAsync(batchOperationNPORights2);

        }


        /// <summary>
        /// Log action into Azure Table
        /// </summary>
        /// <param name="npoID"></param>
        /// <param name="actionType"></param>
        /// <param name="userID"></param>
        /// <param name="actionData"></param>
        public async void AddLog(string npoID, string actionType, string userID, string userName, string actionData)
        {
            CloudTable azTableNPO = tables.GetTableReference(npoID);

            NPOATLog npoNewLog = new NPOATLog("OperationLogs", actionType);
            npoNewLog.UserID = userID;
            npoNewLog.UserName = userName;
            npoNewLog.ActionData = actionData;

            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(npoNewLog);

            // Execute the insert operation.
            await azTableNPO.ExecuteAsync(insertOperation);
        }
    }
}
