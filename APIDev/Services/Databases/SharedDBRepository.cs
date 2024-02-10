using APIDev.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIDev.Models;

namespace APIDev.Services
{
    public class SharedDBRepository :ISharedDBRepository
    {
        private OrganizationContext _context;

        public SharedDBRepository(OrganizationContext context)
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

        public IEnumerable<SiteNames> GetAllSiteNames()
        {
            return _context.SiteNamess.ToList();
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

        


    }
}
