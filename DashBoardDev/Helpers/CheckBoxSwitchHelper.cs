using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Html;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DashBoardDev.Helpers
{
    public class CheckBoxSwitchHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.Content.SetHtmlContent("<label></lable>");
            //base.Process(context, output);
        }


    }
}
