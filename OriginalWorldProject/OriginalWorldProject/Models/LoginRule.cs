using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OriginalWorldProject.Models
{
    public class LoginRule:ActionFilterAttribute
    {
        void LoginCheck(HttpContext context) {
            if (context.Session["Administrator"] == null)
            {
                context.Response.Redirect("/Login/Adm_Login");
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoginCheck(HttpContext.Current);
        }
    }
}