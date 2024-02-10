using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardDev.ViewModels
{
    public class vMNPOCustomFieldsForm
    {
        public enum AllFieldType
        {
            [Display(Name = "TextBox")]
            TextBox,
            [Display(Name = "TextArea")]
            TextArea,
            [Display(Name = "CheckBox")]
            CheckBox,
            [Display(Name = "CheckBoxList")]
            CheckBoxList,
            [Display(Name = "RadioButtonList")]
            RadioButtonList,
            [Display(Name = "Select")]
            Select,
        }

        public class NPOFields
        {
            //public NPOFields()
            //{
            //    FieldType = new List<SelectListItem>();
            //}




            public string NPOID { get; set; }

            //public IEnumerable<SelectListItem> FieldType { get; set; }


            //public AllFieldType? FieldType { get; set; }

            public string FieldType { get; set; }

            [StringLength(50, ErrorMessage = "Le champs doit faire moins de 150 caracteres.")]
            [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
            [DisplayName("Name")]
            public string FieldName { get; set; }

            [StringLength(50, ErrorMessage = "Le champs doit faire moins de 150 caracteres.")]
            [RegularExpression("([a-zA-Z0-9 ,_.'-éèôÉÈ]+)", ErrorMessage = "Seul les lettres et ces signes de ponctuation ,_.'- sont autorises.")]
            [DisplayName("Label")]
            public string FieldLabel { get; set; }

            [DataType(DataType.MultilineText)]
            [DisplayName("Data")]
            public string FieldData { get; set; }

            [DataType(DataType.MultilineText)]
            [DisplayName("Popup")]
            public string FieldPopup { get; set; }


            public bool FieldMandatory { get; set; }

            [DisplayName(null)]
            public bool FieldActivate { get; set; }
            public bool FieldHidden { get; set; }

            public int? FieldOrder { get; set; }

        }

    }
}
