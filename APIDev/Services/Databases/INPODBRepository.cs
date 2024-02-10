using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIDev.Models;
//using FrontEndAdmin.ViewModels;

namespace APIDev.Services
{
    public interface INPODBRepository
    {
        Task<IEnumerable<NPONavBar>> GetAllMyNPO(string userID);

        NPO GetNPOByID(string id);
        
        Task<bool> SaveAll();

        void AddNPO(NPO npo);

        void SetProcessedToDefault(string npoID);
    }
}
