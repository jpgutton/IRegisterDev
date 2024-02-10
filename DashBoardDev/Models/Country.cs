using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DashBoardDev.Models
{
    public class Country
    {
        [Key]
        public string CountryName { get; set; }
        [NotMapped]
        public int StateID { get; set; }
    }
}
