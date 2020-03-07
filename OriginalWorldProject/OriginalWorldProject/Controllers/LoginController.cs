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



        public ActionResult Mem_Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Mem_Login(string Account, string M_Password)
        {
            var All_Mem = db.Member.Where(a => a.Email == Account && a.M_Password == M_Password  || a.Account == Account && a.M_Password == M_Password).FirstOrDefault();
            int val;

            if (All_Mem == null)
            {
                ViewBag.Message = "帳號或密碼錯誤!!";
                return View();
            }
            if (All_Mem.Verify_status == false)
            {
                val = 0;
            }
            else {
                val = 1;
            }
            var Writter_ID = db.Writer.Where(w => w.MemberID == All_Mem.MemberID).Select(w => w.WriterID).FirstOrDefault();

            Session["Member"] = All_Mem;
            Session["Verify_status"] = val;
            Session["Member_Nickname"] = All_Mem.Nickname;
            Session["Member_ID"] = All_Mem.MemberID;
            Session["Writter_ID"] = Writter_ID;
            return RedirectToAction("Index", "Writer_application");
        }


        public ActionResult Mem_Logout()
        {
            Session.Clear();
            return RedirectToAction("Mem_Login", "Login");
        }
    }
}