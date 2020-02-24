using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using OriginalWorldProject.Models;
using OriginalWorldProject.ViewModel;
using PagedList;

namespace OriginalWorldProject.Controllers
{
    [LoginRule]
    public class AnnouncementsController : Controller
    {
        OriginalWorldEntities db = new OriginalWorldEntities();

        public ActionResult Index(string start="", string end="", int page = 1, int pagesize = 10, string announcement_type_id = "", string title = "")
        {
            int pagecurrent = page < 1 ? 1 : page;

            AnnouncementClass announcement = new AnnouncementClass()
            {
                Announcements = db.Announcement.Where(a => a.Announcement_type_ID.Contains(announcement_type_id) && a.Announcement_title.Contains(title)).OrderByDescending(a => a.Announcement_time).ToPagedList(pagecurrent, pagesize).ToList(),
                Announcement_Types = db.Announcement_type.ToList(),
                Administrators = db.Administrator.ToList(),
                AnnouncementsPageList = db.Announcement.Where(a => a.Announcement_type_ID.Contains(announcement_type_id) && a.Announcement_title.Contains(title)).OrderByDescending(a => a.Announcement_time).ToPagedList(pagecurrent, pagesize)
            };


            ViewBag.start = start;
            ViewBag.end = end;
            ViewBag.titletext = title;
            ViewBag.announcement_type_id = announcement_type_id;
            return View(announcement);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DateTime start, DateTime end, string announcement_type_id, string title, int page = 1, int pagesize = 10)
        {
            int pagecurrent = page < 1 ? 1 : page;
            DateTime end_ = end.AddDays(1);
            AnnouncementClass announcement = new AnnouncementClass()
            {
                Announcements = db.Announcement.Where(a => a.Announcement_time >= start && a.Announcement_time <= end_ && a.Announcement_type_ID.Contains(announcement_type_id) && a.Announcement_title.Contains(title)).OrderByDescending(a => a.Announcement_time).ToPagedList(pagecurrent, pagesize).ToList(),
                Announcement_Types = db.Announcement_type.ToList(),
                Administrators = db.Administrator.ToList(),
                AnnouncementsPageList = db.Announcement.Where(a => a.Announcement_time >= start && a.Announcement_time <= end_ && a.Announcement_type_ID.Contains(announcement_type_id) && a.Announcement_title.Contains(title)).OrderByDescending(a => a.Announcement_time).ToPagedList(pagecurrent, pagesize)
            };

            ViewBag.start = start.ToString("yyyy-MM-dd");
            ViewBag.end = end.ToString("yyyy-MM-dd");
            ViewBag.announcement_type_id = announcement_type_id;
            ViewBag.titletext = title;
            return View(announcement);
        }

        public ActionResult Create()
        {
            ViewBag.Ann_class = new SelectList(db.Announcement_type, "Announcement_type_ID", "Announcement_type1");
            var id = db.Announcement.OrderByDescending(m => m.Announcement_ID).Select(m => m.Announcement_ID).FirstOrDefault();
            if (id == 2147483647)
            {
                return View("~/Views/ErrorPage/ID_Full_Error.cshtml");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Announcement announcement)
        {
            int id = db.Announcement.OrderByDescending(m => m.Announcement_ID).Select(m => m.Announcement_ID).FirstOrDefault();
            id++;
            announcement.Announcement_ID = id;
            announcement.Announcement_time = DateTime.Now;
            db.Announcement.Add(announcement);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcement.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = id;
            ViewBag.Ann_Display = announcement.Ann_Display;
            ViewBag.Announcement_type_ID = new SelectList(db.Announcement_type, "Announcement_type_ID", "Announcement_type1", announcement.Announcement_type_ID);
            return View(announcement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Announcement_type_ID = new SelectList(db.Announcement_type, "Announcement_type_ID", "Announcement_type1", announcement.Announcement_type_ID);
            ViewBag.Ann_Display = announcement.Ann_Display;
            return View(announcement);
        }

        public ActionResult Details(int? id) {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcement.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);

        }

        public ActionResult Delete(int? id)
        {
            var announcement = db.Announcement.Where(a => a.Announcement_ID == id).FirstOrDefault();
            db.Announcement.Remove(announcement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}