using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDev.Models
{
    public class NPORights : TableEntity
    {
        public NPORights(string clubid, string userid)
        {
            this.PartitionKey = clubid;
            this.RowKey = userid;
        }

        public NPORights() { }

        public string UserName { get; set; }
        public string AddDate { get; set; }
    }
}

