using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OriginalWorldProject.Models
{
    public class H_LoginRule : ActionFilterAttribute
    {
        void LoginCheck(HttpContext context)
        {
            var aut = context.Session["Authority"];
            switch (aut) {
                case 0:
                    context.Response.Redirect("/Login/Low_Authority");
                    break;
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoginCheck(HttpContext.Current);
        }
    }
}