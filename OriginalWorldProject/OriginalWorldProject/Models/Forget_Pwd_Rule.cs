using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OriginalWorldProject.Models
{
    public class Forget_Pwd_Rule : ActionFilterAttribute
    {
        void LoginCheck(HttpContext context)
        {
            if (context.Session["mail"] == null)
            {
                context.Response.Redirect("/Login/Low_Authority");
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoginCheck(HttpContext.Current);
        }
    }
}