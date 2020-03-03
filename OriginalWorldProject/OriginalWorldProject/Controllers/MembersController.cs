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
    [LoginRule]
    public class MembersController : Controller
    {
        OriginalWorldEntities db = new OriginalWorldEntities();


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
        public ActionResult Index(int Age_star, int Age_end, string M_status, string Gender, string Writter_qualifications, string Account,string Verify_status, int page = 1, int pagesize = 10)
        {
            Birthday_Interval birthday_Interval = new Birthday_Interval();
            DateTime Age_bth_star = birthday_Interval.Birthday_interval(Age_star).Date;
            DateTime Age_bth_end = birthday_Interval.Birthday_interval(Age_end).AddDays(-365).Date;

            int pagecurrent = page < 1 ? 1 : page;
            var list = db.Member.Where(m => m.Gender.ToString().Contains(Gender) && m.Birthday <= Age_bth_star && m.Birthday >= Age_bth_end && m.M_status.ToString().Contains(M_status) && m.Writter_qualifications.ToString().Contains(Writter_qualifications)&&m.Verify_status.ToString().Contains(Verify_status) && Account == "" ? m.Account.Contains(Account) : m.Account.Equals(Account)).OrderBy(a => a.MemberID).ToPagedList(pagecurrent, pagesize).ToList();
            var pagelist = list.ToPagedList(pagecurrent, pagesize);

            ViewBag.Age_star = Age_star;
            ViewBag.Age_end = Age_end;
            ViewBag.M_status = M_status;
            ViewBag.Gender = Gender;
            ViewBag.Writter_qualifications = Writter_qualifications;
            ViewBag.Account = Account;
            ViewBag.Verify_status = Verify_status;
            return View(pagelist);
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
            ViewBag.Writter_qualifications = member.Writter_qualifications;
            ViewBag.M_status = member.M_status;
            ViewBag.password = member.M_Password;
            ViewBag.Gender = member.Gender;
            ViewBag.Verify_status = member.Verify_status;
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Member member)
        {
            if (ModelState.IsValid) { 
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }


        public ActionResult Edit_Member(string id)
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
            ViewBag.Writter_qualifications = member.Writter_qualifications;
            ViewBag.M_status = member.M_status;
            ViewBag.password = member.M_Password;
            ViewBag.Gender = member.Gender;
            ViewBag.Verify_status = member.Verify_status;

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Member(Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

    }
}
