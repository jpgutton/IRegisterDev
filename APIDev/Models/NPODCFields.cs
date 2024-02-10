using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIDev.Models
{
    public class NPODCFields
    {
        public List<NPOFields> DCNPOFields { get; set; }

        public static implicit operator List<object>(NPODCFields v)
        {
            throw new NotImplementedException();
        }
    }
}
