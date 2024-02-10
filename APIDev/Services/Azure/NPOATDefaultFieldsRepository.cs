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
    public class NPOATDefaultFieldsRepository : INPOATDefaultFieldsRepository
    {
        private OrganizationContext _context;

        public CloudTable tableUserRights;
        public CloudTable tableNPORights;
        public CloudTable tableNPOCustomfields;

        public CloudStorageAccount tableStorageAccount;

        public CloudTableClient tables;

        private string _tableConnection;

        private const string DateFormat = "yyyyMMdd - HH:mm:ss";


        public NPOATDefaultFieldsRepository(OrganizationContext context, IOptions<AzureTablesOptions> tableConnection)
        {
            _context = context;
            _tableConnection = tableConnection.Value.AzTableConnection;

            tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);

            tables = tableStorageAccount.CreateCloudTableClient();

            tableUserRights = tables.GetTableReference("UserRights");
            tableNPORights = tables.GetTableReference("NPORights");
        }


        #region Default Fields
        /// <summary>
        /// Retrieve all my NPO default fields
        /// </summary>
        /// <param name="npoID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NPOFields>> GetAllMyNPODefaultFields(string userID, string userName, string npoID)
        {
            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
            CloudTable azTableNPO = tableNPO.GetTableReference(npoID);

            // Construct the query operation for all field entities where PartitionKey="DefaultNPOFields".
            TableQuery<NPOAZFields> query = new TableQuery<NPOAZFields>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "DefaultField"));

            // Print the fields for each customer.
            TableContinuationToken token = null;

            var npoDefaultFields = new List<NPOFields>();

            do
            {
                TableQuerySegment<NPOAZFields> resultSegment = await azTableNPO.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (NPOAZFields entity in resultSegment.Results)
                {
                    NPOFields npoFields = new NPOFields();
                    npoFields.NPOID = npoID;
                    npoFields.FieldType = entity.FieldType;
                    npoFields.FieldName = entity.RowKey;
                    npoFields.FieldLabel = entity.FieldLabel;
                    npoFields.FieldData = entity.FieldData;
                    npoFields.FieldPopup = entity.FieldPopup;
                    npoFields.FieldMandatory = entity.FieldMandatory;
                    npoFields.FieldActivate = entity.FieldActivate;
                    npoFields.FieldHidden = entity.FieldHiden;
                    npoFields.FieldOrder = entity.FieldOrder;
                    npoDefaultFields.Add(npoFields);
                }
            } while (token != null);

            AddLog(npoID, "Read Defaults Field - " + DateTime.Now.ToUniversalTime().ToString(DateFormat), userID, userName, "Read Defaults fields");

            return npoDefaultFields;
        }


        /// <summary>
        /// Retrieve all my NPO default fields
        /// </summary>
        /// <param name="npoID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NPOFields>> GetNPODefaultFields(string npoID)
        {
            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
            CloudTable azTableNPO = tableNPO.GetTableReference("DefaultFields");

            // Construct the query operation for all field entities where PartitionKey="DefaultNPOFields".
            TableQuery<NPOAZFields> query = new TableQuery<NPOAZFields>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "DefaultNPOFields"));

            // Print the fields for each customer.
            TableContinuationToken token = null;

            var npoDefaultFields = new List<NPOFields>();

            do
            {
                TableQuerySegment<NPOAZFields> resultSegment = await azTableNPO.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (NPOAZFields entity in resultSegment.Results)
                {
                    NPOFields npoFields = new NPOFields();
                    npoFields.NPOID = npoID;
                    npoFields.FieldType = entity.FieldType;
                    npoFields.FieldName = entity.RowKey;
                    npoFields.FieldLabel = entity.FieldLabel;
                    npoFields.FieldData = entity.FieldData;
                    npoFields.FieldPopup = entity.FieldPopup;
                    npoFields.FieldMandatory = entity.FieldMandatory;
                    npoFields.FieldActivate = entity.FieldActivate;
                    npoFields.FieldHidden = entity.FieldHiden;
                    npoFields.FieldOrder = entity.FieldOrder;
                    npoDefaultFields.Add(npoFields);
                }
            } while (token != null);


            return npoDefaultFields;
        }


        /// <summary>
        /// Post Initial Default Fields values
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<bool> PostDefaultFields(string userID, string userName, List<NPOFields> list)
        {
            // retrieve the NPO ID
            string npoID = list[0].NPOID;

            // Get the Azure Table for NPO
            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
            CloudTable azTableNPO = tableNPO.GetTableReference(list[0].NPOID);


            #region Insert fields into AZ Table

            TableBatchOperation batchOperations = new TableBatchOperation();

            int i = 0;

            foreach (NPOFields npoDefaultFields in list)
            {
                NPOAZFields blah = new NPOAZFields("DefaultField", npoDefaultFields.FieldName);
                blah.FieldActivate = npoDefaultFields.FieldActivate;
                blah.FieldData = npoDefaultFields.FieldData;
                blah.FieldHiden = npoDefaultFields.FieldHidden;
                blah.FieldLabel = npoDefaultFields.FieldLabel;
                blah.FieldMandatory = npoDefaultFields.FieldMandatory;
                blah.FieldPopup = npoDefaultFields.FieldPopup;
                blah.FieldType = npoDefaultFields.FieldType;
                blah.FieldOrder = npoDefaultFields.FieldOrder;

                batchOperations.Add(TableOperation.InsertOrReplace(blah));

                i++;
            }
            await azTableNPO.ExecuteBatchAsync(batchOperations);
            #endregion

            #region Validate number of default field entries
            var npoDefaultFieldsValidate = new List<NPOFields>();

            // Construct the query operation for all customer entities where PartitionKey="DefaultNPOFields".
            TableQuery<NPOAZFields> query = new TableQuery<NPOAZFields>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "DefaultField"));

            // Print the fields for each customer.
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<NPOAZFields> resultSegment = await azTableNPO.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (NPOAZFields entity in resultSegment.Results)
                {
                    NPOFields npoFields = new NPOFields();
                    npoFields.NPOID = npoID;
                    npoFields.FieldType = entity.FieldType;
                    npoFields.FieldName = entity.RowKey;
                    npoFields.FieldLabel = entity.FieldLabel;
                    npoFields.FieldData = entity.FieldData;
                    npoFields.FieldPopup = entity.FieldPopup;
                    npoFields.FieldMandatory = entity.FieldMandatory;
                    npoFields.FieldActivate = entity.FieldActivate;
                    npoFields.FieldHidden = entity.FieldHiden;
                    npoFields.FieldOrder = entity.FieldOrder;
                    npoDefaultFieldsValidate.Add(npoFields);
                }
            } while (token != null);
            #endregion

            // Validate if all fields were added to the table
            if (i == npoDefaultFieldsValidate.Count())
            {
                AddLog(npoID, "Add Defaults Field - " + DateTime.Now.ToUniversalTime().ToString(DateFormat), userID, userName, "Defaults fields");
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}
