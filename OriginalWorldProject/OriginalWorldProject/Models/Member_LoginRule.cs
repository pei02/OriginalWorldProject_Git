using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OriginalWorldProject.Models
{
    public class Member_LoginRule : ActionFilterAttribute
    {
        void LoginCheck(HttpContext context)
        {
            if (context.Session["Member"] == null)
            {
                context.Response.Redirect("/Login/Mem_Login");
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoginCheck(HttpContext.Current);
        }
    }
}