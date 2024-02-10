using AutoMapper;
using DashBoardDev.DAL;
using DashBoardDev.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
    public class AzureTables
    {
        #region Fields

        private readonly IOptions<AppSettings> appSettings;
        private readonly OrganizationContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;

        private string _tableConnection;

        #endregion

        #region Constructors

        public AzureTables(
            IOptions<AppSettings> appSettings,
            OrganizationContext context,
            IMemoryCache memoryCache,
            IOptions<AzureTablesOptions> tableConnection,
            IMapper mapper)
        {
            this.appSettings = appSettings;
            _context = context;
            _memoryCache = memoryCache;
            _tableConnection = tableConnection.Value.AzTableConnection;
            _mapper = mapper;
        }

        #endregion


        /// <summary>
        /// Create a new table on Azure based on the NPO model
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task AddNewNPOAzTable(string tableName)
        {
            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);

            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();

            // Get a reference to a table named after the ClubID
            CloudTable azTableNPO = tableNPO.GetTableReference(tableName);

            await azTableNPO.CreateAsync();
        }


        /// <summary>
        /// Used to validate the uniqueness of the chosen name for the site to be used
        /// </summary>
        /// <param name="organizationSiteName"></param>
        /// <returns></returns>
        public async Task<String> CheckOrganizationSiteNameExists(string organizationSiteName)
        {
            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);

            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();

            // Get a reference to a table named after the ClubID
            CloudTable azTableNPO = tableNPO.GetTableReference("NPOSitename");

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve("SiteName", organizationSiteName);

            // Execute the retrieve operation.
            TableResult retrievedResult = await azTableNPO.ExecuteAsync(retrieveOperation);

            string isExisting;

            if (retrievedResult.Result != null)
            {
                isExisting = "Present";
                return isExisting;
            }
            else
            {
                isExisting = "NotPresent";
                return isExisting;
            }
        }





    }
}
