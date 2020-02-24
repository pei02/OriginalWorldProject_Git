using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OriginalWorldProject.Models;
using OriginalWorldProject.App_Code;
using System.Configuration;
using System.Data.SqlClient;
using GoogleRecaptcha;
using System.Data.Common;
using PagedList;

namespace OriginalWorldProject.Controllers
{
    [LoginRule]
    public class AdministratorsController : Controller
    {
        OriginalWorldEntities db = new OriginalWorldEntities();

        [H_LoginRule]
        public ActionResult Index(int page = 1, int pagesize = 10, string Authority = "e")
        {
            var list = db.Administrator.ToList();
            int pagecurrent = page < 1 ? 1 : page;
            var pagelist = list.ToPagedList(pagecurrent, pagesize);
            ViewBag.Authority = Authority;
            return View(pagelist);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string Account, string Adm_name, string Authority, int page = 1, int pagesize = 10)
        {

            int pagecurrent = page < 1 ? 1 : page;
            var list = db.Administrator.Where(a => a.Authority.ToString().Contains(Authority) && a.Adm_name.Contains(Adm_name) && Account == "" ? a.Account.Contains(Account) : a.Account.Equals(Account)).OrderBy(a => a.Administrator_ID).ToPagedList(pagecurrent, pagesize).ToList();
            var pagelist = list.ToPagedList(pagecurrent, pagesize);

            ViewBag.Authority = Authority;
            ViewBag.Account = Account;
            ViewBag.Adm_name = Adm_name;
            return View(pagelist);

        }


        [H_LoginRule]
        public ActionResult Create()
        {
            /******************Get Random Password*****************/
            RandomPassword randomPassword = new RandomPassword();
            string pwd = randomPassword.RandomPwd();
            ViewBag.RandomPwd = pwd;
            /**********************Get New ID**********************/
            var Adm_Desc = from m in db.Administrator
                           orderby m.Administrator_ID descending
                           select m.Administrator_ID;
            string old_id = Adm_Desc.FirstOrDefault();
            if (old_id == "A999")
            {
                return View("~/Views/ErrorPage/ID_Full_Error.cshtml");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Administrator administrator, FormCollection form)
        {
            /**********************Get New ID**********************/
            var Adm_Desc = from m in db.Administrator
                           orderby m.Administrator_ID descending
                           select m.Administrator_ID;
            string old_id = Adm_Desc.FirstOrDefault();

            if (old_id == null)
            {
                old_id = "A000";
            }

            NewID newID = new NewID();
            string newid = newID.NewID_fuction(old_id, "A", 1, 3);

            Google_reCAPTCHA google_ReCAPTCHA = new Google_reCAPTCHA();
            Boolean reCAPTCHA = google_ReCAPTCHA.reCAPTCHA();

            if (reCAPTCHA == true)
            {
                if (ModelState.IsValid)
                {
                    administrator.Administrator_ID = newid;
                    db.Administrator.Add(administrator);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.msg = "發生錯誤,請重新執行!!";
            RandomPassword randomPassword = new RandomPassword();
            string pwd = randomPassword.RandomPwd();
            ViewBag.RandomPwd = pwd;
            return View(administrator);
        }

        [H_LoginRule]
        public ActionResult Edit(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var administrator_ID = db.Administrator.Where(a => a.Administrator_ID == id).FirstOrDefault();

            if (administrator_ID == null)
            {
                return HttpNotFound();
            }

            ViewBag.Authority = administrator_ID.Authority;
            return View(administrator_ID);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Administrator administrator)
        {
            try
            {
                SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OriginalWorldConnectionString"].ConnectionString);
                SqlCommand Cmd = new SqlCommand("updateAdm", Conn);

                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("Administrator_ID", administrator.Administrator_ID);
                Cmd.Parameters.AddWithValue("Adm_name", administrator.Adm_name);
                Cmd.Parameters.AddWithValue("Account", administrator.Account);
                Cmd.Parameters.AddWithValue("Password", administrator.A_Password);
                Cmd.Parameters.AddWithValue("Authority", administrator.Authority);
                Cmd.Parameters.AddWithValue("Confirm_pwd", administrator.Confirm_pwd);

                Conn.Open();
                Cmd.ExecuteNonQuery();
                Conn.Close();

                return RedirectToAction("Index");
            }
            catch (DbException ex)
            {
                ViewBag.ex = ex.Message;
            }

            return View(administrator);
        }

        [H_LoginRule]
        public ActionResult Details(string id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrator.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        [H_LoginRule]
        public ActionResult Delete(string id)
        {
            var administrator_ID = db.Administrator.Where(a => a.Administrator_ID == id).FirstOrDefault();
            db.Administrator.Remove(administrator_ID);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Adm_edit_password(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var administrator_ID = db.Administrator.Where(a => a.Administrator_ID == id).FirstOrDefault();

            if (administrator_ID == null)
            {
                return HttpNotFound();
            }

            var b = Session["Adm_ID"].ToString();
            int result = string.Compare(b, id);
            switch (result)
            {
                case 0:
                    break;
                default:
                    return View("~/Views/Login/Low_Authority.cshtml");
            }
            return View(administrator_ID);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Adm_edit_password(Administrator administrator)
        {
            Session["Adm_edit_password_ID"] = administrator.Administrator_ID;
            try
            {
                SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OriginalWorldConnectionString"].ConnectionString);
                SqlCommand Cmd = new SqlCommand("updateAdm", Conn);

                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("Administrator_ID", administrator.Administrator_ID);
                Cmd.Parameters.AddWithValue("Adm_name", administrator.Adm_name);
                Cmd.Parameters.AddWithValue("Account", administrator.Account);
                Cmd.Parameters.AddWithValue("Password", administrator.A_Password);
                Cmd.Parameters.AddWithValue("Authority", administrator.Authority);
                Cmd.Parameters.AddWithValue("Confirm_pwd", administrator.Confirm_pwd);

                Conn.Open();
                Cmd.ExecuteNonQuery();
                Conn.Close();

                return Content("<script>alert('密碼修改成功!!');</script>");

            }
            catch (DbException ex)
            {
                ViewBag.ex = ex.Message;
            }

            return Content("<script>alert('發生錯誤,請重新再試!!');</script>");

        }

        public JsonResult check_adm_name(string Adm_name)
        {
            return Json(!db.Administrator.Any(x => x.Adm_name.ToLower() == Adm_name.ToLower()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult check_adm_account(string Account)
        {
            return Json(!db.Administrator.Any(x => x.Account.ToLower() == Account.ToLower()), JsonRequestBehavior.AllowGet);
        }

    }
}
