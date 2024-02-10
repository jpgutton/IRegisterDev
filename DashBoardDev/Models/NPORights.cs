using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashBoardDev.Models
{
    public class NPORights : TableEntity
    {
        public NPORights(string npoid, string userid)
        {
            this.PartitionKey = npoid;
            this.RowKey = userid;
        }

        public NPORights() { }  

        public string UserName { get; set; }
        public string AddDate { get; set; }
    }
}

