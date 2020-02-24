using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OriginalWorldProject.Models;

namespace OriginalWorldProject.Controllers
{
    public class LoginController : Controller
    {
        OriginalWorldEntities db = new OriginalWorldEntities();

        public ActionResult Adm_Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Adm_Login(string Account, string A_Password)
        {
            var All_adm = db.Administrator.Where(a => a.Account == Account && a.A_Password == A_Password).FirstOrDefault();
            int aut;

            if (All_adm == null)
            {
                ViewBag.Message = "帳號或密碼錯誤！！";
                return View();
            }
            if (All_adm.Authority == false)
            {
                aut = 0;
            }
            else
            {
                aut = 1;
            }
            //string second = All_adm.Adm_name;

            Session["Administrator"] = All_adm;
            Session["Adm_ID"] = All_adm.Administrator_ID;
            Session["Authority"] = aut;
            Session["Administrator_name"] = All_adm.Adm_name.Substring(1, 1);
            return RedirectToAction("Index", "Announcements");
        }

        public ActionResult Low_Authority()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Adm_Login", "Login");
        }
    }
}