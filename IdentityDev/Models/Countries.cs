using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityDev.Models
{
    public class Countries : TableEntity
    {
        public Countries(string countries, string shortname)
        {
            this.PartitionKey = countries;
            this.RowKey = shortname;
        }

        public Countries() { }

        public string Name { get; set; }

    }
}
