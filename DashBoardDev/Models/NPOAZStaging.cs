using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardDev.Models
{
    public class NPOAZStaging : TableEntity
    {
        public NPOAZStaging(string npoID, string contentID)
        {
            this.PartitionKey = npoID;
            this.RowKey = contentID;
        }

        public NPOAZStaging() { }

        public string Data { get; set; }
    }
}
