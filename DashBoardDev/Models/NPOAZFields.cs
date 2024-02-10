using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardDev.Models
{
    public class NPOAZFields : TableEntity
    {
        public NPOAZFields(string npoID, string FieldID)
        {
            this.PartitionKey = npoID;
            this.RowKey = FieldID;
        }

        public NPOAZFields() { }

        public bool FieldActivate { get; set; }
        public string FieldData { get; set; }
        public bool FieldHiden { get; set; }
        public string FieldLabel { get; set; }
        public bool FieldMandatory { get; set; }
        public string FieldPopup { get; set; }
        public string FieldType { get; set; }
        public int FieldOrder { get; set; }

    }
}
