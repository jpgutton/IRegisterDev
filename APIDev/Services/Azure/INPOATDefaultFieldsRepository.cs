using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIDev.Models;

namespace APIDev.Services
{
    public interface INPOATDefaultFieldsRepository
    {
        Task<IEnumerable<NPOFields>> GetAllMyNPODefaultFields(string userID, string userName, string npoID);

        Task<IEnumerable<NPOFields>> GetNPODefaultFields(string npoID);

        Task<bool> PostDefaultFields(string userID, string userName, List<NPOFields> list);
    }
}
