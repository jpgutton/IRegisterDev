using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using IdentityModel.Client;
using System.Text;
using System.Threading;
using Boilerplate.AspNetCore;
using Boilerplate.AspNetCore.Filters;
using Microsoft.Extensions.Options;
using DashBoardDev.Constants;
using DashBoardDev.Services;
using DashBoardDev.Settings;
using Microsoft.Extensions.Localization;
using DashBoardDev.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace DashBoardDev.Controllers
{
    public class DashboardsController : Controller
    {
        #region Fields

        private readonly IOptions<AppSettings> appSettings;
        private readonly IStringLocalizer<DashboardsController> _localizer;
        private readonly IMemoryCache _memoryCache;

        #endregion

        #region Constructors

        public DashboardsController(
            IStringLocalizer<DashboardsController> localizer,
            IOptions<AppSettings> appSettings,
            IMemoryCache memoryCache)
        {
            _localizer = localizer;
            this.appSettings = appSettings;
            _memoryCache = memoryCache;
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        //[HttpGet("index", Name = DashboardsControllerRoute.GetIndex)]
        public IActionResult Index()
        {
            ViewData["Message"] = "Secure page.";

            var claims = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();

            var dateOfBirth = User.Claims.FirstOrDefault(c => c.Type == "given_name").Value;

            //Test for session
            try
            {
                //string blah = _memoryCache.Get("ClubID").ToString();

                string blah2 = HttpContext.Session.GetString("NewClubID");
            }
            catch { }

            return this.View(DashboardsControllerAction.Index);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult MyNPOsListNavigationBar()
        {
            return ViewComponent("MyNPOsListNavigationBar");
        }

    }
}
