using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIDev.Models;

namespace APIDev.Services
{
    public interface INPOATSharedRepository
    {
        void AddLog(string npoID, string actionType, string userID, string userName, string actionData);

        void CreateTable(string NPOSiteID);

        Task<bool> NPOCheckRights(string npoID, string userID);

        void AddNewNPORole(string ClubID, string NPOSiteID, string UserID, string UserName);
    }
}
