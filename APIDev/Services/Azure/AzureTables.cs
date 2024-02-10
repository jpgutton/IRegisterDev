using AutoMapper;
using APIDev.DAL;
//using FrontEndAdmin.Settings;
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

namespace APIDev.Services
{
    public class AzureTables
    {
        #region Fields

        //private readonly IOptions<AppSettings> appSettings;
        private readonly OrganizationContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;

        private string _tableConnection;

        #endregion

        #region Constructors

        public AzureTables(
            //IOptions<AppSettings> appSettings,
            OrganizationContext context,
            IMemoryCache memoryCache,
            IOptions<AzureTablesOptions> tableConnection,
            IMapper mapper)
        {
            //this.appSettings = appSettings;
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
        /// Create a new table for the new NPO
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task TableAddNPOFormFields(string tableName)
        {
            CloudStorageAccount tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);

            CloudTableClient tableNPO = tableStorageAccount.CreateCloudTableClient();

            // Get a reference to a table named after the ClubID
            CloudTable azTableNPO = tableNPO.GetTableReference(tableName);

            await azTableNPO.CreateAsync();
        }
    }
}
