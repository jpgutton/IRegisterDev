using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;

namespace DashBoardDev
{
    public static class HtmlHelpers
    {

        public static string IsSelected(this IHtmlHelper htmlHelper, string controller = null, string action = null, string cssClass = null)
        {
            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"];
            string currentController = (string)htmlHelper.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        //public static IHtmlContent CheckBoxSwitch2<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> expression)
        //{
        //    return string.Format("alhdsfkhasfd");

        //}

        public static string CheckBoxSwitch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> expression, string action)
        {
            return string.Format("alhdsfkhasfd");
        }

        //public static HtmlString CheckBoxSwitchFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        //{



        //    return HtmlString.(tagBuilder.ToString(TagRenderMode.Normal));


        //}



        //public static string CheckBoxSwitch(this IHtmlHelper htmlHelper, string action, string cssClass = null)
        //{
        //    string blah = "";
        //    string blah2 = "";
        //    string blah3 = "";
        //    //if (!String.IsNullOrEmpty(action))
        //    //{ 
        //    //blah = "<label class=\"onoffswitch - label\" for=\"" + action + "\"<span class=\"onoffswitch - inner\"></span><span class=\"onoffswitch-switch\"></span></label>";
        //    //}

        //    //blah = "<label class=\"onoffswitch - label\" for=\"" + action + "\"<span class=\"onoffswitch - inner\"></span><span class=\"onoffswitch-switch\"></span></label>";
        //    blah = "<label class=\"onoffswitch - label\" for=\"";
        //    blah2 = "\"<span class=\"onoffswitch - inner\"></span></label>";
        //    blah3 = blah + action + blah2;


        //    return blah3;
        //}



        public static string PageClass(this IHtmlHelper htmlHelper)
        {
            string currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

    }



}
