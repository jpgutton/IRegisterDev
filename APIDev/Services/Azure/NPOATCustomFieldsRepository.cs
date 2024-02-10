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
    public class NPOATCustomFieldsRepository : INPOATCustomFieldsRepository
    {
        private OrganizationContext _context;

        public CloudTable tableUserRights;
        public CloudTable tableNPORights;
        public CloudTable tableNPOCustomfields;

        public CloudStorageAccount tableStorageAccount;

        public CloudTableClient tables;

        private string _tableConnection;

        private const string DateFormat = "yyyyMMdd - HH:mm:ss";


        public NPOATCustomFieldsRepository(OrganizationContext context, IOptions<AzureTablesOptions> tableConnection)
        {
            _context = context;
            _tableConnection = tableConnection.Value.AzTableConnection;

            tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);

            tables = tableStorageAccount.CreateCloudTableClient();

            tableUserRights = tables.GetTableReference("UserRights");
            tableNPORights = tables.GetTableReference("NPORights");
        }


        #region Custom Fields

        /// <summary>
        /// Retrieve the list of custom fields for a NPO
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="npoID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NPOFields>> GetAllMyNPOCustomFields(string userID, string npoID)
        {
            TableQuery<NPOAZFields> query;
            TableContinuationToken token = null;

            tableNPOCustomfields = tables.GetTableReference(npoID);

            // Construct the query operation for all customer entities where PartitionKey="DefaultNPOFields".
            query = new TableQuery<NPOAZFields>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CustomNPOFields"));

            // Print the fields for each customer.
            var npoDefaultFields = new List<NPOFields>();

            do
            {
                TableQuerySegment<NPOAZFields> resultSegment = await tableNPOCustomfields.ExecuteQuerySegmentedAsync(query, token);
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
        /// 
        /// </summary>
        /// <param name="NPOFields"></param>
        /// <returns></returns>
        public async Task<bool> AddNPOCustomField(NPOFields npoFields)
        {
            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();

            CloudTable azTableNPO = tableNPO.GetTableReference(npoFields.NPOID);

            string fieldName = npoFields.FieldName;
            fieldName = fieldName.Replace("\"", "");
            fieldName = fieldName.Replace("\'", "");
            fieldName = fieldName.Replace("-", "");
            fieldName = fieldName.RemoveDiacritics();
            fieldName = fieldName.Replace(" ", "");

            npoFields.FieldName = fieldName;



            NPOAZFields npoAZFields = new NPOAZFields("CustomNPOFields", fieldName);
            npoAZFields.FieldLabel = npoFields.FieldLabel;
            npoAZFields.FieldActivate = npoFields.FieldActivate;
            npoAZFields.FieldData = npoFields.FieldData;
            npoAZFields.FieldHiden = npoFields.FieldHidden;
            npoAZFields.FieldLabel = npoFields.FieldLabel;
            npoAZFields.FieldMandatory = npoFields.FieldMandatory;
            npoAZFields.FieldOrder = npoFields.FieldOrder;
            npoAZFields.FieldPopup = npoFields.FieldPopup;
            npoAZFields.FieldType = npoFields.FieldType;

            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(npoAZFields);

            // Execute the insert operation.
            await azTableNPO.ExecuteAsync(insertOperation);

            // Create a retrieve operation to verify if user has the right on the NPO
            TableOperation retrieveOperation = TableOperation.Retrieve<NPOAZFields>("CustomNPOFields", npoFields.FieldName);

            // Execute the retrieve operation.
            TableResult retrievedResult = await azTableNPO.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                CloudTable azTableNPOUniqueFieldName = tableNPO.GetTableReference(npoFields.NPOID + "fieldname");
                NPOUniqueFieldName newField = new NPOUniqueFieldName(npoFields.NPOID, npoFields.FieldName);

                // Create the TableOperation that inserts the customer entity.
                TableOperation insertOperation2 = TableOperation.Insert(newField);

                // Execute the insert operation.
                await azTableNPOUniqueFieldName.ExecuteAsync(insertOperation2);

                return true;
            }
            return false;
        }


        /// <summary>
        /// Retrieve Custom NPO Field Data
        /// </summary>
        /// <param name="npoID"></param>
        /// <param name="npoFieldName"></param>
        /// <returns></returns>
        public async Task<NPOFields> npoField(string npoID, string npoFieldName)
        {
            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
            CloudTable azTableNPO = tableNPO.GetTableReference(npoID);

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<NPOAZFields>("CustomNPOFields", npoFieldName);

            // Execute the retrieve operation.
            TableResult retrievedResult = await azTableNPO.ExecuteAsync(retrieveOperation);

            if(retrievedResult != null)
            {
                NPOFields npoFieldData = new NPOFields();
                npoFieldData.FieldActivate = ((NPOAZFields)retrievedResult.Result).FieldActivate;
                npoFieldData.FieldData = ((NPOAZFields)retrievedResult.Result).FieldData;
                npoFieldData.FieldHidden = ((NPOAZFields)retrievedResult.Result).FieldHiden;
                npoFieldData.FieldLabel = ((NPOAZFields)retrievedResult.Result).FieldLabel;
                npoFieldData.FieldMandatory = ((NPOAZFields)retrievedResult.Result).FieldMandatory;
                npoFieldData.FieldName = ((NPOAZFields)retrievedResult.Result).RowKey;
                npoFieldData.FieldOrder = ((NPOAZFields)retrievedResult.Result).FieldOrder;
                npoFieldData.FieldPopup = ((NPOAZFields)retrievedResult.Result).FieldPopup;
                npoFieldData.FieldType = ((NPOAZFields)retrievedResult.Result).FieldType;
                npoFieldData.NPOID = ((NPOAZFields)retrievedResult.Result).PartitionKey;

                return npoFieldData;
            }
            NPOFields npoEmpty = new NPOFields();
            return npoEmpty;
        }


        /// <summary>
        /// Update an NPO custom field
        /// </summary>
        /// <param name="npoFields"></param>
        /// <returns></returns>
        public async Task<bool> UpdateNPOCustomField(NPOFields npoFields)
        {
            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
            CloudTable azTableNPO = tableNPO.GetTableReference(npoFields.NPOID);

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<NPOAZFields>("CustomNPOFields", npoFields.FieldName);

            // Execute the retrieve operation.
            TableResult retrievedResult = await azTableNPO.ExecuteAsync(retrieveOperation);

            NPOAZFields updateNPOField = (NPOAZFields)retrievedResult.Result;

            if(updateNPOField != null)
            {
                updateNPOField.FieldActivate = npoFields.FieldActivate;
                updateNPOField.FieldData = npoFields.FieldData;
                updateNPOField.FieldHiden = npoFields.FieldHidden;
                updateNPOField.FieldLabel = npoFields.FieldLabel;
                updateNPOField.FieldMandatory = npoFields.FieldMandatory;
                updateNPOField.FieldOrder = npoFields.FieldOrder;
                updateNPOField.FieldPopup = npoFields.FieldPopup;
                updateNPOField.FieldType = npoFields.FieldType;

                // Create the InsertOrReplace TableOperation.
                TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(updateNPOField);

                // Execute the operation.
                await azTableNPO.ExecuteAsync(insertOrReplaceOperation);

                return true;
            }
            return false;
        }


        /// <summary>
        /// Delete a Custom NPO Field
        /// </summary>
        /// <param name="npoID"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public async Task<bool> DeleteNPOCustomField(string npoID, string fieldName)
        {
            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);
            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();
            CloudTable azTableNPO = tableNPO.GetTableReference(npoID);

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<NPOAZFields>("CustomNPOFields", fieldName);

            // Execute the retrieve operation.
            TableResult retrievedResult = await azTableNPO.ExecuteAsync(retrieveOperation);

            NPOAZFields deleteNPOField = (NPOAZFields)retrievedResult.Result;

            if (deleteNPOField != null)
            {
                // Create the InsertOrReplace TableOperation.
                TableOperation deleteOperation = TableOperation.Delete(deleteNPOField);

                // Execute the operation.
                await azTableNPO.ExecuteAsync(deleteOperation);

                return true;
            }
            return false;

        }




        #endregion
    }
}
