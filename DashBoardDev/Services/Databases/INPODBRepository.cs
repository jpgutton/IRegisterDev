using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardDev.Models;
using DashBoardDev.ViewModels;

namespace DashBoardDev.Services
{
    public interface INPODBRepository
    {

        SiteNames GetSiteName(string sitename);
        void AddSiteName(SiteNames sitename);

        Task<bool> SaveAll();
        //Task<bool> SaveChangeAsync();

        //Task<bool> SaveNPO();
        void AddNPO(NPO npo);
        //IEnumerable<NPO> GetAllNPOforUserX();
        //NPO GetNPO(string npoName, string groupID);

    }
}
