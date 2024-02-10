using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIDev.Models
{
    public class NPONavBar
    {
        public string ClubID { get; set; }
        public string UserID { get; set; }
        public string Organization { get; set; }
        public string Processed { get; set; }
        public string GroupID { get; set; }
        public string NPOSiteID { get; set; }
    }
}
