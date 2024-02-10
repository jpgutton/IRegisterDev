using AutoMapper;
using DashBoardDev.DAL;
using DashBoardDev.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardDev.Services
{
    public class AzureTablesOptions
    {
        public string AzTableConnection { get; set; }

    }




}


