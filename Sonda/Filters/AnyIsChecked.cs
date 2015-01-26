using Probea.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sonda.Filters
{
    public class AnyIsChecked : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ProbeViewModel model = (ProbeViewModel)filterContext.ActionParameters.First().Value;
            if (!model.Answers.Any(x => x.IsChecked))
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary 
                { 
                    { "controller", filterContext.RouteData.Values["controller"] }, 
                    { "action", filterContext.RouteData.Values["action"] } 
                });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}