using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardDev.Models
{
    public class NPOUniqueFieldName : TableEntity
    {
        public NPOUniqueFieldName(string NPOID, string FieldName)
        {
            this.PartitionKey = NPOID;
            this.RowKey = FieldName;
        }

        public NPOUniqueFieldName() { }
    }
}
