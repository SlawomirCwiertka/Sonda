using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sonda.Filters
{
    public class SetCookieProbe : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var probeIsChecked = filterContext.HttpContext.Request.Cookies["ProbeChecked"];
            if (probeIsChecked != null && filterContext.HttpContext.Request.Form["PublicationDate"] == probeIsChecked.Value)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary 
                { 
                    { "controller", filterContext.RouteData.Values["controller"] }, 
                    { "action", filterContext.RouteData.Values["action"] } 
                });
            }
            else
            {
                HttpCookie newCokkie = new HttpCookie("ProbeChecked");
                newCokkie.Value = filterContext.HttpContext.Request.Form["PublicationDate"];
                filterContext.HttpContext.Response.Cookies.Add(newCokkie);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}