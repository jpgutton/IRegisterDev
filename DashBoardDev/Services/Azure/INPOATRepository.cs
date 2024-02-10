using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardDev.Models;

namespace DashBoardDev.Services
{
    public interface INPOATRepository
    {
        //void AddLog(string npoID, string actionType, string userID, string userName, string actionData);

        Task<bool> NPOCheckRights(string npoID, string userID);
    }
}
