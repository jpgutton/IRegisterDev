using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardDev.ViewModels
{
    public class vMNPONavBar
    {
        public string NPOID { get; set; }
        public string UserID { get; set; }
        public string Organization { get; set; }
        public string Processed { get; set; }
        public string GroupID { get; set; }
        public string NPOSiteID { get; set; }
    }
}
