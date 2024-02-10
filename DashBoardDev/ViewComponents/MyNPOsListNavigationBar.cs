using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DashBoardDev.ViewModels;
using Newtonsoft.Json;

namespace DashBoardDev.ViewComponents
{
    public class MyNPOsListNavigationBar : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var content = await client.GetStringAsync("http://localhost:45101/api/nponavbar");

            var myNPOs = JsonConvert.DeserializeObject<List<vMNPONavBar>>(content);

            return View("Default", myNPOs);
        }
    }
}
