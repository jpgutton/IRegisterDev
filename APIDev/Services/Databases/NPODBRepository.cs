using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIDev.DAL;
using APIDev.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIDev.Services
{
    public class NPODBRepository : INPODBRepository
    {
        private OrganizationContext _context;

        public CloudTable tableUserRights;
        public CloudTable tableNPORights;
        public CloudTable tableNPOCustomfields;
        public CloudStorageAccount tableStorageAccount;
        public CloudTableClient tables;
        private string _tableConnection;

        public NPODBRepository(OrganizationContext context, IOptions<AzureTablesOptions> tableConnection)
        {
            _context = context;
            _tableConnection = tableConnection.Value.AzTableConnection;

            tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);

            tables = tableStorageAccount.CreateCloudTableClient();

            tableUserRights = tables.GetTableReference("UserRights");
            tableNPORights = tables.GetTableReference("NPORights");

        }
        

        /// <summary>
        /// Retrieve all my NPO for NavBar
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NPONavBar>> GetAllMyNPO(string userID)
        {
            List<string> list = new List<string>();

            TableQuery<NPORights> query = new TableQuery<NPORights>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userID));

            // Print the fields for each customer.
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<NPORights> resultSegment = await tableUserRights.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (NPORights entity in resultSegment.Results)
                {
                    list.Add(entity.RowKey);
                }
            } while (token != null);
 
            var blahnpo = from d in _context.NPOs
                          join id in list
                          on d.GroupID equals id
                          select new NPONavBar
                          {
                              ClubID = d.ClubID,
                              UserID = d.UserID,
                              Organization = d.Organization,
                              GroupID = d.GroupID,
                              Processed = d.Processed,
                              NPOSiteID = d.NPOSiteID
                          };

            return blahnpo;
        }


        /// <summary>
        /// Retrieve an NPO by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NPO GetNPOByID(string id)
        {
            return _context.NPOs
                .Where(t => t.ClubID == id)
                .FirstOrDefault();
        }

        
        /// <summary>
        /// Save the new NPO
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAll()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }


        /// <summary>
        /// Add the new NPO model
        /// </summary>
        /// <param name="npo"></param>
        public void AddNPO(NPO npo)
        {
            _context.Add(npo);
        }


        /// <summary>
        /// Chaneg processed field in the NPO DB to default
        /// </summary>
        /// <param name="npoID"></param>
        public async void SetProcessedToDefault(string npoID)
        {
            NPO npo = _context.NPOs
                .Where(t => t.NPOSiteID == npoID)
                .FirstOrDefault();

            if(npo.Processed != "default" || npo.Processed != "Yes")
            {
                npo.Processed = "default";

                await _context.SaveChangesAsync();
            }
        }


    }
}
