using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardDev.Constants;
using DashBoardDev.DAL;
using DashBoardDev.Models;
using DashBoardDev.Settings;
using DashBoardDev.ViewModels;

namespace DashBoardDev.Services
{
    public class NPODBRepository : INPODBRepository
    {
        private OrganizationContext _context;

        public NPODBRepository(OrganizationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Check for existing sitename for uniqueness during NPO Creation
        /// </summary>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public SiteNames GetSiteName(string sitename)
        {
            return _context.SiteNamess
                .SingleOrDefault(m => m.SiteName == sitename);
        }

        /// <summary>
        /// Add new sitename in DB
        /// </summary>
        /// <param name="siteName"></param>
        public void AddSiteName(SiteNames siteName)
        {
            _context.Add(siteName);
        }

        public async Task<bool> SaveAll()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }



        public void AddNPO(NPO npo)
        {
            _context.Add(npo);
        }



    }
}
