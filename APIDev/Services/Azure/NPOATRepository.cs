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
    public class NPOATRepository : INPOATRepository
    {
        private OrganizationContext _context;

        public CloudStorageAccount tableStorageAccount;

        public CloudTableClient tables;

        private string _tableConnection;

        private const string DateFormat = "yyyyMMdd - HH:mm:ss";


        public NPOATRepository(OrganizationContext context, IOptions<AzureTablesOptions> tableConnection)
        {
            _context = context;
            _tableConnection = tableConnection.Value.AzTableConnection;

            tableStorageAccount = CloudStorageAccount.Parse(_tableConnection);

            tables = tableStorageAccount.CreateCloudTableClient();
        }
    }
}
