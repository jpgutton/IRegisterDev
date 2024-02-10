using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardDev.Models;

namespace DashBoardDev.ViewModels
{
    public class vMNPOCustomFields
    {
        public List<NPOFields> NPOFields { get; set; }
        public NPOFields SelectedFields { get; set; }
        public string DisplayMode { get; set; }

    }
}
