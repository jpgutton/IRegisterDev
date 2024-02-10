using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIDev.Models
{
    public class NPOATLog : TableEntity
    {
        public NPOATLog(string npoID, string ActionType)
        {
            this.PartitionKey = npoID;
            this.RowKey = ActionType;
        }

        public NPOATLog() { }

        public string UserID { get; set; }
        public string UserName { get; set; }

        public string ActionData { get; set; }
    }
}
