using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardDev.Models
{
    public class State
    {
        [Key]
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
    }
}
