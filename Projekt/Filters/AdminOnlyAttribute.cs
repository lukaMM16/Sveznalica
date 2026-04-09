using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class AdminOnlyAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var role = filterContext.HttpContext.Session["Role"] as string;

        if (role != "Admin")
        {
            filterContext.Result = new RedirectResult("~/Auth/Login");
            return;
        }

        base.OnActionExecuting(filterContext);
    }
}
