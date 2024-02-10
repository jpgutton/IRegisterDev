using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityDev.Models
{
    public class LogSession : TableEntity
    {
        public LogSession(string eventtype, string datekey)
        {
            this.PartitionKey = eventtype;
            this.RowKey = datekey;
        }

        public LogSession() { }

        public string UserName { get; set; }
        public string IP { get; set; }

        public string UserAgent { get; set; }

    }
}
