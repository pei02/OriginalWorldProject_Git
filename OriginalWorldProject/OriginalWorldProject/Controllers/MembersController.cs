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
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


namespace OriginalWorldProject.Controllers
{

    public class MembersController : Controller
    {
        OriginalWorldEntities db = new OriginalWorldEntities();

        [LoginRule]
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
        public ActionResult Index(int Age_star, int Age_end, string M_status, string Gender, string Writter_qualifications, string Account, string Verify_status, int page = 1, int pagesize = 10)
        {
            Birthday_Interval birthday_Interval = new Birthday_Interval();
            DateTime Age_bth_star = birthday_Interval.Birthday_interval(Age_star).Date;
            DateTime Age_bth_end = birthday_Interval.Birthday_interval(Age_end).AddDays(-365).Date;

            int pagecurrent = page < 1 ? 1 : page;
            var list = db.Member.Where(m => m.Gender.ToString().Contains(Gender) && m.Birthday <= Age_bth_star && m.Birthday >= Age_bth_end && m.M_status.ToString().Contains(M_status) && m.Writter_qualifications.ToString().Contains(Writter_qualifications) && m.Verify_status.ToString().Contains(Verify_status) && Account == "" ? m.Account.Contains(Account) : m.Account.Equals(Account)).OrderBy(a => a.MemberID).ToPagedList(pagecurrent, pagesize).ToList();
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

        [LoginRule]
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
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Writter_qualifications = member.Writter_qualifications;
            ViewBag.M_status = member.M_status;
            ViewBag.password = member.M_Password;
            ViewBag.Gender = member.Gender;
            ViewBag.Verify_status = member.Verify_status;
            return View(member);
        }

        [Forget_Pwd_Rule]
        public ActionResult Edit_pwd(string id)
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
            var b = Session["id"].ToString();
            int result = string.Compare(b, id);
            switch (result)
            {
                case 0:
                    break;
                default:
                    return View("~/Views/Login/Low_Authority.cshtml");
            }
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_pwd(Member member)
        {
            var pwd_vaild = db.Member.Where(w => w.MemberID == member.MemberID).Select(w => w.M_Password).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (member.M_Password == pwd_vaild) {
                    ViewBag.pwdvaild = "密碼不可與原始密碼相同";
                    return View(member);
                }
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return Content("<script>alert('修改密碼成功!! 請重新登入');window.location.href='/Login/Mem_Login';</script>");
            }
            return View(member);
        }



        [Member_LoginRule]
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
            var b = Session["Member_ID"].ToString();
            int result = string.Compare(b, id);
            switch (result)
            {
                case 0:
                    break;
                default:
                    return View("~/Views/Login/Low_Authority.cshtml");
            }

            ViewBag.Writter_qualifications = member.Writter_qualifications;
            ViewBag.M_status = member.M_status;
            ViewBag.password = member.M_Password;
            ViewBag.Gender = member.Gender;
            ViewBag.Verify_status = member.Verify_status;
            ViewBag.email = member.Email;
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Member(Member member)
        {

            var email = db.Member.Where(w => w.MemberID == member.MemberID).Select(w => w.Email).FirstOrDefault();
            var email_vaild = db.Member.Where(w => w.Email == member.Email).Select(w => w.Email).FirstOrDefault();

            ViewBag.password = member.M_Password;
            ViewBag.email = member.Email;

            if (ModelState.IsValid)
            {
                if (member.Email != email)
                {
                    if (email == null)
                    {
                        member.Verify_status = false;
                        db.Entry(member).State = EntityState.Modified;
                        db.SaveChanges();
                        return Content("<script>alert('請重新執行驗證');window.location.href='/Signup/Verification_usermail?memberID=" + member.MemberID + "';</script>");
                    }
                    else { 
                        ViewBag.email_vaild = "信箱已被註冊";
                        ViewBag.email = member.Email;
                        return View(member);
                    }
                }
               
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        public ActionResult Forget_pwd()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Forget_pwd(string email)
        {
            var email_vaild = db.Member.Where(w => w.Email == email).Select(w => w.Email).FirstOrDefault();
            var id = db.Member.Where(w => w.Email == email).Select(w => w.MemberID).FirstOrDefault();
            if (email_vaild != null)
            {
                return RedirectToAction("Forget_pwdmail", new { memberID = id , Email= email });
            }
            ViewBag.email_vaild = "信箱不存在,請重新輸入!!";
            return View(email);
        }

        public ActionResult Forget_pwdmail(string memberID,string Email)
        {

            if (memberID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Member.Find(memberID);
            if (member == null)
            {
                return HttpNotFound();
            }
            Session["id"] = memberID;
            Session["mail"] = member.Email;
            Session["name"] = member.Nickname;
            Session["result"] = "";
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Forget_pwdmail(string verify_code)
        {
            string result;
            if (Session["Verificationcode"].Equals(verify_code) == false)
            {
                ViewBag.verify_code = "驗證碼錯誤!!";
                result = "0";
                Session["result"] = result;
                return View();
            }
            else { 
             result = "1";
             Session["result"] = result;
            }
            return RedirectToAction("Edit_pwd",new { id= Session["id"] });
        }
        public void VerificationEmail()
        {

            RandomPassword ver_code = new RandomPassword();
            string Verificationcode = ver_code.RandomVerificationcode();

            Session["Verificationcode"] = Verificationcode;

            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            bool enableSSL = true;
            string emailFrom = "im.writter0221@gmail.com";
            string password = "asd7821778@";

            string emailto = Session["mail"].ToString();
            string subject = "您好" + Session["name"].ToString() + ",執行忘記密碼,請先驗證您的信箱";
            string body = "您的驗證碼是" + Verificationcode + "<br>執行重新取得驗證碼此組驗證碼將失效";


            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom, "寫書人", System.Text.Encoding.UTF8);
                mail.To.Add(emailto);
                mail.Subject = subject;
                mail.Body = body;

                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
        }

    }
}
