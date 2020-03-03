using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OriginalWorldProject.Models;
using OriginalWorldProject.ViewModel;
using OriginalWorldProject.App_Code;
using PagedList;



namespace OriginalWorldProject.Controllers
{
    public class Writer_applicationController : Controller
    {
        OriginalWorldEntities db = new OriginalWorldEntities();

        [LoginRule]
        public ActionResult Index(int page = 1, int pagesize = 10, string Doa_star = "", string Doa_end = "", string Pseudonym = "", string Works_name = "", string Approval_status_ID = "A1")
        {
            int pagecurrent = page < 1 ? 1 : page;

            Writer_applicationClass writer_Application = new Writer_applicationClass()
            {
                Writer_Applications = db.Writer_application.Where(w => w.Approval_status_ID == Approval_status_ID && w.Pseudonym.Contains(Pseudonym) && w.Works_name.Contains(Works_name)).OrderByDescending(w => w.Date_of_Application).ToPagedList(pagecurrent, pagesize).ToList(),
                Administrators = db.Administrator.ToList(),
                Members = db.Member.ToList(),
                Approval_Statuses = db.Approval_status.ToList(),
                Writer_applicationPageList = db.Writer_application.Where(w => w.Approval_status_ID == Approval_status_ID && w.Pseudonym.Contains(Pseudonym) && w.Works_name.Contains(Works_name)).OrderByDescending(w => w.Date_of_Application).ToPagedList(pagecurrent, pagesize)
            };

            string[] wa_id = { "A1", "A2", "A3", "A4" };
            ViewBag.WA1 = wa_count(wa_id[0]);
            ViewBag.WA2 = wa_count(wa_id[1]);
            ViewBag.WA3 = wa_count(wa_id[2]);
            ViewBag.WA4 = wa_count(wa_id[3]);
            Session["new"] = wa_count(wa_id[0]);


            ViewBag.Doa_star = Doa_star;
            ViewBag.Doa_end = Doa_end;
            ViewBag.Pseudonym = Pseudonym;
            ViewBag.Works_name = Works_name;
            ViewBag.Approval_status_ID = Approval_status_ID;
            ViewBag.count = writer_Application.Writer_applicationPageList.Count();

            return View(writer_Application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DateTime Doa_star, DateTime Doa_end, int page = 1, int pagesize = 10, string Works_name = "", string Approval_status_ID = "A1", string Pseudonym = "")
        {
            int pagecurrent = page < 1 ? 1 : page;
            DateTime Doa_end_ = Doa_end.AddDays(1);

            Writer_applicationClass writer_Application = new Writer_applicationClass()
            {
                Writer_Applications = db.Writer_application.Where(w => w.Approval_status_ID == Approval_status_ID && w.Pseudonym.Contains(Pseudonym) && w.Works_name.Contains(Works_name) &&
                                                                       w.Date_of_Application >= Doa_star && w.Date_of_Application <= Doa_end_).OrderByDescending(w => w.Date_of_Application).ToPagedList(pagecurrent, pagesize).ToList(),

                Administrators = db.Administrator.ToList(),
                Members = db.Member.ToList(),
                Approval_Statuses = db.Approval_status.ToList(),
                Writer_applicationPageList = db.Writer_application.Where(w => w.Approval_status_ID == Approval_status_ID && w.Pseudonym.Contains(Pseudonym) && w.Works_name.Contains(Works_name) &&
                                                                       w.Date_of_Application >= Doa_star && w.Date_of_Application <= Doa_end_).OrderByDescending(w => w.Date_of_Application).ToPagedList(pagecurrent, pagesize)

            };

            string[] wa_id = { "A1", "A2", "A3", "A4" };
            ViewBag.WA1 = wa_count(wa_id[0]);
            ViewBag.WA2 = wa_count(wa_id[1]);
            ViewBag.WA3 = wa_count(wa_id[2]);
            ViewBag.WA4 = wa_count(wa_id[3]);

            ViewBag.Doa_star = Doa_star.ToString("yyyy-MM-dd");
            ViewBag.Doa_end = Doa_end.ToString("yyyy-MM-dd");
            ViewBag.Pseudonym = Pseudonym;
            ViewBag.Works_name = Works_name;
            ViewBag.count = writer_Application.Writer_applicationPageList.Count();
            ViewBag.Approval_status_ID = Approval_status_ID;
            return View(writer_Application);
        }

        [Member_LoginRule]
        public ActionResult Create()
        {
            ViewBag.Approval_status_ID = new SelectList(db.Approval_status, "Approval_status_ID", "Approval_status1");
            Google_reCAPTCHA google_ReCAPTCHA = new Google_reCAPTCHA();
            Boolean reCAPTCHA = google_ReCAPTCHA.reCAPTCHA();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Writer_application writer_application, string Pseudonym)
        {
            var Pseudonym_vaild = db.Writer_application.Where(w => w.Pseudonym == Pseudonym).Select(w => w.Pseudonym).FirstOrDefault();
            var w_id = db.Writer_application.Where(w => w.MemberID == writer_application.MemberID).Select(w => w.Pseudonym).ToList();

            if (w_id.Where(w => w.Equals(Pseudonym_vaild)).FirstOrDefault() != null) Pseudonym_vaild = null;

            int New_id = db.Writer_application.OrderByDescending(w => w.Writer_application_ID).Select(w => w.Writer_application_ID).FirstOrDefault();
            New_id++;
            Google_reCAPTCHA google_ReCAPTCHA = new Google_reCAPTCHA();
            Boolean reCAPTCHA = google_ReCAPTCHA.reCAPTCHA();

            if (reCAPTCHA == true)
            {
                if (ModelState.IsValid && Pseudonym_vaild == null)
                {
                    writer_application.Date_of_Application = DateTime.Now;
                    writer_application.Approval_status_ID = "A1";
                    writer_application.Writer_application_ID = New_id;
                    db.Writer_application.Add(writer_application);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            if (Pseudonym_vaild != null) ViewBag.Pseudonym_vaild = "筆名已被註冊!!";

            ViewBag.msg = "發生錯誤,請重新執行!!";
            ViewBag.Approval_status_ID = new SelectList(db.Approval_status, "Approval_status_ID", "Approval_status1", writer_application.Approval_status_ID);
            return View(writer_application);
        }
        [LoginRule]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Writer_application writer_application = db.Writer_application.Find(id);
            if (writer_application == null)
            {
                return HttpNotFound();
            }
            ViewBag.Approval_status_ID = writer_application.Approval_status_ID;
            return View(writer_application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Writer_application writer_application)
        {
            ViewBag.Approval_status_ID = writer_application.Approval_status_ID;
            if (ModelState.IsValid)
            {
                writer_application.Review_date = DateTime.Now;
                db.Entry(writer_application).State = EntityState.Modified;
                db.SaveChanges();
                if (writer_application.Approval_status_ID == "A3") {
                    Boolean result = writter_create(writer_application.Pseudonym, writer_application.MemberID, writer_application.Writer_application_ID);
                    if (result == true)
                    {
                        Member_W_q(writer_application.MemberID);
                        return Content("<script>alert('新增作家成功');window.location.href='/Writer_application/index?Approval_status_ID=A3';</script>");
                    }
                    else {
                        return View(writer_application);
                    } 
                }
                return RedirectToAction("Index");
            }
            return View(writer_application);
        }

        [LoginRule]
        public bool writter_create(string Pseudonym, string MemberID, int Writer_application_ID)
        {
            var writter_vaild = db.Writer.Where(w => w.Pseudonym == Pseudonym).FirstOrDefault();
            var wa_vaild = db.Writer.Where(w => w.Writer_application_ID == Writer_application_ID).FirstOrDefault();
            var mem_vaild = db.Writer.Where(w => w.MemberID == MemberID).FirstOrDefault();
            string old_id = db.Writer.OrderByDescending(w => w.WriterID).Select(w => w.WriterID).FirstOrDefault();
            if (old_id == null) old_id = "W000000000";
            NewID newID = new NewID();
            Writer writer = new Writer();
            if (writter_vaild == null && wa_vaild == null && mem_vaild == null) {
                writer.WriterID = newID.NewID_fuction(old_id, "W", 1, 9);
                writer.Pseudonym = Pseudonym;
                writer.W_status = false;
                writer.MemberID = MemberID;
                writer.Writer_application_ID = Writer_application_ID;
                db.Writer.Add(writer);
                db.SaveChanges();
                return true;
            }
            if (writter_vaild != null || wa_vaild != null || mem_vaild != null) {
              if(writter_vaild != null) TempData["writter_vaild"] = "筆名已被註冊!!";
                if (wa_vaild != null) TempData["wa_vaild"] = "申請書編號已被其他會員使用!!";
                if (mem_vaild != null) TempData["mem_vaild"] = "此會員已經是作家,不須重複申請!!";
            }
            return false;
        }

        public void Member_W_q(string id)
        {
            Member member = db.Member.Find(id);
            member.MemberID = id;
            member.Writter_qualifications = true;
            db.Entry(member).State = EntityState.Modified;
            db.SaveChanges();
        }

        public int wa_count(string wa_id)
        {
            int wa_id_count = db.Writer_application.Where(w => w.Approval_status_ID == wa_id).Select(w => w.Approval_status_ID).Count();
            return wa_id_count;
        }

    }
}
