using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIDev.Models;

namespace APIDev.Services
{
    public interface ISharedDBRepository
    {
        SiteNames GetSiteName(string sitename);
        IEnumerable<SiteNames> GetAllSiteNames();
        void AddSiteName(SiteNames sitename);
        Task<bool> SaveAll();
    }
}
