using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OriginalWorldProject.Controllers
{
    public class ErrorPageController : Controller
    {
        // GET: ErrorPage
        public ActionResult NotFound404()
        {
            return View();
        }
        public ActionResult Error400()
        {
            return View();
        }
        public ActionResult ID_Full_Error()
        {
            return View();
        }
    }
}