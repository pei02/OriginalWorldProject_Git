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
using System.Data.SqlClient;
using System.Configuration;
using GoogleRecaptcha;
using System.Data.Common;
using PagedList;


namespace OriginalWorldProject.Controllers
{
    public class MembersController : Controller
    {
        OriginalWorldEntities db = new OriginalWorldEntities();

        // GET: Members
        public ActionResult Index(int? Age_star, int? Age_end, int page = 1, int pagesize = 10)
        {
            var list = db.Member.ToList();
            int pagecurrent = page < 1 ? 1 : page;
            var pagelist = list.ToPagedList(pagecurrent, pagesize);


            ViewBag.Age_star = Age_star;
            ViewBag.Age_end = Age_end;
            return View(pagelist);
        }

        [HttpPost]
        public ActionResult Index(int Age_star, int Age_end, string M_status, string Gender, string Writter_qualifications, string Account, int page = 1, int pagesize = 10)
        {
            Birthday_Interval birthday_Interval = new Birthday_Interval();
            DateTime Age_bth_star = birthday_Interval.Birthday_interval(Age_star).Date;
            DateTime Age_bth_end = birthday_Interval.Birthday_interval(Age_end).AddDays(-365).Date;

            int pagecurrent = page < 1 ? 1 : page;
            var list = db.Member.Where(m => m.Gender.ToString().Contains(Gender) && m.Birthday <= Age_bth_star && m.Birthday >= Age_bth_end && m.M_status.ToString().Contains(M_status) && m.Writter_qualifications.ToString().Contains(Writter_qualifications) && Account == "" ? m.Account.Contains(Account) : m.Account.Equals(Account)).OrderBy(a => a.MemberID).ToPagedList(pagecurrent, pagesize).ToList();
            var pagelist = list.ToPagedList(pagecurrent, pagesize);

            ViewBag.Age_star = Age_star;
            ViewBag.Age_end = Age_end;
            ViewBag.M_status = M_status;
            ViewBag.Gender = Gender;
            ViewBag.Writter_qualifications = Writter_qualifications;
            ViewBag.Account = Account;
            return View(pagelist);
        }

        public ActionResult Create()
        {
            string id = db.Member.OrderByDescending(m => m.MemberID).Select(m => m.MemberID).FirstOrDefault();
            if (id == "M999999999")
            {
                return View("~/Views/ErrorPage/ID_Full_Error.cshtml");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Member member)
        {
            string id = db.Member.OrderByDescending(m => m.MemberID).Select(m => m.MemberID).FirstOrDefault();

            if (id == null)
            {
                id = "M000000000";
            }
            NewID newID = new NewID();
            string new_ID = newID.NewID_fuction(id, "M", 1, 9);
            if (ModelState.IsValid)
            {
                member.MemberID = new_ID;
                member.M_status = false;
                member.Writter_qualifications = false;
                db.Member.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Member member)
        {

            try
            {
                SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OriginalWorldConnectionString"].ConnectionString);
                SqlCommand Cmd = new SqlCommand("updateMem", Conn);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("MemberID", member.MemberID);
                Cmd.Parameters.AddWithValue("Nickname", member.Nickname);
                Cmd.Parameters.AddWithValue("Account", member.Account);
                Cmd.Parameters.AddWithValue("M_Password", member.M_Password);
                Cmd.Parameters.AddWithValue("Phone", member.Phone);
                Cmd.Parameters.AddWithValue("Email", member.Email);
                Cmd.Parameters.AddWithValue("Gender", member.Gender);
                Cmd.Parameters.AddWithValue("Birthday", member.Birthday);
                Cmd.Parameters.AddWithValue("M_status", member.M_status);
                Cmd.Parameters.AddWithValue("Writter_qualifications", member.Writter_qualifications);
                Cmd.Parameters.AddWithValue("Confirm_pwd", member.Confirm_pwd);

                Conn.Open();
                Cmd.ExecuteNonQuery();
                Conn.Close();

                return RedirectToAction("Index");
            }
            catch (DbException ex)
            {
                ViewBag.ex = ex.Message;
            }

            return View(member);
        }

        public JsonResult check_Nickname(string Nickname)
        {
            return Json(!db.Member.Any(x => x.Nickname.ToLower() == Nickname.ToLower()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult check_M_account(string Account)
        {
            return Json(!db.Member.Any(x => x.Account.ToLower() == Account.ToLower()), JsonRequestBehavior.AllowGet);
        }

    }
}
