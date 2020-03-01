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
using PagedList;



namespace OriginalWorldProject.Controllers
{
    public class Writer_applicationController : Controller
    {
        OriginalWorldEntities db = new OriginalWorldEntities();


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
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Writer_application writer_application)
        {
            int New_id = db.Writer_application.OrderByDescending(w => w.Writer_application_ID).Select(w => w.Writer_application_ID).FirstOrDefault();
            New_id++;
            if (ModelState.IsValid)
            {
                writer_application.Date_of_Application = DateTime.Now;
                writer_application.Approval_status_ID = "A1";
                writer_application.Writer_application_ID = New_id;
                db.Writer_application.Add(writer_application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Approval_status_ID = new SelectList(db.Approval_status, "Approval_status_ID", "Approval_status1", writer_application.Approval_status_ID);
            return View(writer_application);
        }

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
            ViewBag.Approval_status_ID = new SelectList(db.Approval_status, "Approval_status_ID", "Approval_status1", writer_application.Approval_status_ID);
            return View(writer_application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Writer_application writer_application)
        {
            if (ModelState.IsValid)
            {
                writer_application.Review_date = DateTime.Now;
                db.Entry(writer_application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.Approval_status_ID = new SelectList(db.Approval_status, "Approval_status_ID", "Approval_status1", writer_application.Approval_status_ID);
            return View(writer_application);
        }

        public int wa_count(string wa_id)
        {
            int wa_id_count = db.Writer_application.Where(w => w.Approval_status_ID == wa_id).Select(w => w.Approval_status_ID).Count();
            return wa_id_count;
        }

        public JsonResult check_Pseudonym(string Pseudonym)
        {
            return Json(!db.Writer_application.Any(x => x.Pseudonym.ToLower() == Pseudonym.ToLower()), JsonRequestBehavior.AllowGet);
        }

    }
}
