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
    [LoginRule]
    public class WritersController : Controller
    {
        OriginalWorldEntities db = new OriginalWorldEntities();


        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            int pagecurrent = page < 1 ? 1 : page;
            WritterClass writter = new WritterClass() {
                writers = db.Writer.OrderByDescending(w=>w.WriterID).ToList(),
                writer_Applications = db.Writer_application.ToList(),
                WritersPageList = db.Writer.OrderByDescending(w => w.WriterID).ToPagedList(pagecurrent, pagesize)
            };
           
            return View(writter);
        }

        [HttpPost]
        public ActionResult Index( int page = 1, int pagesize = 10,string Pseudonym="",string W_status="e")
        {
            int pagecurrent = page < 1 ? 1 : page;
            WritterClass writter = new WritterClass()
            {
                writers = db.Writer.Where(w=>Pseudonym==""?w.Pseudonym.Contains(Pseudonym):w.Pseudonym.Equals(Pseudonym)&&w.W_status.ToString().Contains(W_status)).OrderByDescending(w => w.WriterID).ToPagedList(pagecurrent,pagesize).ToList(),
                writer_Applications = db.Writer_application.ToList(),
                WritersPageList = db.Writer.Where(w => Pseudonym == "" ? w.Pseudonym.Contains(Pseudonym) : w.Pseudonym.Equals(Pseudonym) && w.W_status.ToString().Contains(W_status)).OrderByDescending(w => w.WriterID).ToPagedList(pagecurrent, pagesize)
            };
            ViewBag.Pseudonym = Pseudonym;
            ViewBag.W_status = W_status;
            return View(writter);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Writer writer = db.Writer.Find(id);
            if (writer == null)
            {
                return HttpNotFound();
            }
            ViewBag.W_status = writer.W_status;
            return View(writer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Writer writer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(writer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.W_status = writer.W_status;
            return View(writer);
        }

    }
}
