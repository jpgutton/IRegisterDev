using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIDev.Models;

namespace APIDev.Services
{
    public interface INPOATCustomFieldsRepository
    {
        Task<IEnumerable<NPOFields>> GetAllMyNPOCustomFields(string userID, string npoID);

        Task<bool> AddNPOCustomField(NPOFields npoFields);

        Task<bool> UpdateNPOCustomField(NPOFields npoFields);

        Task<bool> DeleteNPOCustomField(string npoID, string fieldName);
    }
}
