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
    public class WorksController : Controller
    {
        OriginalWorldEntities db = new OriginalWorldEntities();

        public ActionResult Index(int page = 1, int pagesize = 10)
        {
            int pagecurrent = page < 1 ? 1 : page;
            WorksClass worksClass = new WorksClass()
            {
                Works = db.Works.OrderByDescending(W => W.WorksID).ToList(),
                Work_Statuses = db.Work_status.ToList(),
                WorksPageList = db.Works.OrderByDescending(W => W.WorksID).ToPagedList(pagecurrent, pagesize)
            };
            return View(worksClass);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Works works = db.Works.Find(id);
            if (works == null)
            {
                return HttpNotFound();
            }
            return View(works);
        }


        [Member_LoginRule]
        public ActionResult Create()
        {
            var type = db.Work_type.Select(t => t.WorktypeID).ToList();
            var type_name = db.Work_type.Select(t => t.Worktype).ToList();
            var Type_table_id = db.Type_table.OrderByDescending(t => t.AM_ID).Select(t => t.AM_ID).FirstOrDefault();

            ViewBag.type = type;
            ViewBag.type_name = type_name;

            if (Type_table_id == 2147483647)
            {
                return View("~/Views/ErrorPage/ID_Full_Error.cshtml");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string Worksname, string WriterID, HttpPostedFileBase Cover, List<string> type)
        {

            Works works = new Works();
            string old_id = db.Works.OrderByDescending(m => m.WorksID).Select(m => m.WorksID).FirstOrDefault();
            var type_ = db.Work_type.Select(t => t.WorktypeID).ToList();
            var type_name = db.Work_type.Select(t => t.Worktype).ToList();
            var num = 0;

            ViewBag.type = type_;
            ViewBag.type_name = type_name;


            if (old_id == null)
            {
                old_id = "T000000000";
            }

            NewID newID = new NewID();
            string new_id = newID.NewID_fuction(old_id, "T", 1, 9);
            
                if (ModelState.IsValid)
                {
                    works.WorksID = new_id;
                    works.Worksname = Worksname;

                    if (Cover != null)
                    {
                        works.Cover = new byte[Cover.ContentLength];
                        Cover.InputStream.Read(works.Cover, 78, Cover.ContentLength - 78);
                    }

                    works.Serialtime = DateTime.Now;
                    works.Endtime = null;
                    works.WorkstatusID = "WS1";
                    works.WriterID = WriterID;

                    db.Works.Add(works);
                    db.SaveChanges();
                    foreach (var i in type)
                    {
                        Create_Type_table(type[num], new_id);
                        num++;
                    }


                    return RedirectToAction("Index");
                }
            

            return View(works);
        }

     
        public void Create_Type_table(string type, string WorksID)
        {
            int id = db.Type_table.OrderByDescending(w => w.AM_ID).Select(w => w.AM_ID).FirstOrDefault();
            id++;
           
            Type_table type_Table = new Type_table();
            type_Table.AM_ID = id;
            type_Table.WorksID = WorksID;
            type_Table.WorktypeID = type;
            db.Type_table.Add(type_Table);
            db.SaveChanges();
        }


        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Works works = db.Works.Find(id);
            if (works == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkstatusID = new SelectList(db.Work_status, "WorkstatusID", "Workstatus", works.WorkstatusID);
            ViewBag.WriterID = new SelectList(db.Writer, "WriterID", "Pseudonym", works.WriterID);
            return View(works);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Works works)
        {
            if (ModelState.IsValid)
            {
                db.Entry(works).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WorkstatusID = new SelectList(db.Work_status, "WorkstatusID", "Workstatus", works.WorkstatusID);
            ViewBag.WriterID = new SelectList(db.Writer, "WriterID", "Pseudonym", works.WriterID);
            return View(works);
        }
    }
}
