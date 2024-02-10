using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardDev.Models
{
    public class NPOAZProd : TableEntity
    {
        public NPOAZProd(string npoID, string contentID)
        {
            this.PartitionKey = npoID;
            this.RowKey = contentID;
        }

        public NPOAZProd() { }

        public string Data { get; set; }
    }
}
