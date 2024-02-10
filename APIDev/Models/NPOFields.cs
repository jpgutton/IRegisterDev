using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIDev.Models
{
    public class NPOFields
    {
        public string NPOID { get; set; }
        public string FieldType { get; set; }
        public string FieldName { get; set; }
        public string FieldLabel { get; set; }
        public string FieldData { get; set; }
        public string FieldPopup { get; set; }
        public bool FieldMandatory { get; set; }
        public bool FieldActivate { get; set; }
        public bool FieldHidden { get; set; }
        public int FieldOrder { get; set; }

    }
}
