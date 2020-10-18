using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OriginalWorldProject.Models;
using OriginalWorldProject.App_Code;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Data.Entity;

namespace OriginalWorldProject.Controllers
{
    public class SignupController : Controller
    {
        OriginalWorldEntities db = new OriginalWorldEntities();
        public ActionResult Signup()
        {
            string id = db.Member.OrderByDescending(m => m.MemberID).Select(m => m.MemberID).FirstOrDefault();
            if (id == "M999999999")
            {
                return View("~/Views/ErrorPage/ID_Full_Error.cshtml");
            }
            Google_reCAPTCHA google_ReCAPTCHA = new Google_reCAPTCHA();
            Boolean reCAPTCHA = google_ReCAPTCHA.reCAPTCHA();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(string nickname, string account, string m_password, string email, DateTime birthday, Boolean gender, string confirm_pwd)
        {

            string id = db.Member.OrderByDescending(m => m.MemberID).Select(m => m.MemberID).FirstOrDefault();
            string nickname_vaild = db.Member.Where(m => m.Nickname == nickname).Select(m => m.Nickname).FirstOrDefault();
            string account_vaild = db.Member.Where(m => m.Account == account).Select(m => m.Account).FirstOrDefault();
            string email_vaild = db.Member.Where(m => m.Email == email).Select(m => m.Email).FirstOrDefault();

            if (id == null)
            {
                id = "M000000000";
            }
            NewID newID = new NewID();
            string new_ID = newID.NewID_fuction(id, "M", 1, 9);
            Google_reCAPTCHA google_ReCAPTCHA = new Google_reCAPTCHA();
            Boolean reCAPTCHA = google_ReCAPTCHA.reCAPTCHA();

            Member member = new Member();

            if (reCAPTCHA == true)
            {
                if (nickname_vaild == null && account_vaild == null && email_vaild == null && confirm_pwd == m_password)
                {
                    member.MemberID = new_ID;
                    member.Nickname = nickname;
                    member.Account = account;
                    member.M_Password = m_password;
                    member.Verify_status = false;
                    member.Email = email;
                    member.Gender = gender;
                    member.Birthday = birthday;
                    member.M_status = false;
                    member.Writter_qualifications = false;
                    member.Confirm_pwd = confirm_pwd;

                    db.Member.Add(member);
                    db.SaveChanges();
                    return RedirectToAction("Verification_usermail", new { memberID = new_ID });
                }
            }
            if (nickname_vaild != null || account_vaild != null || email_vaild != null || confirm_pwd != m_password)
            {
                if (nickname_vaild != null) ViewBag.Nickname = "暱稱、";
                if (account_vaild != null) ViewBag.Account = "帳號、";
                if (email_vaild != null) ViewBag.Email = "信箱，";
                if (confirm_pwd != m_password) ViewBag.Confirm_pwd = "密碼不相符!!";
            }

            return View(member);
        }

        public ActionResult Verification_usermail(string memberID)
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
          
            Session["usermail"] = member.Email;
            Session["username"] = member.Nickname;

            return View(member);
        }

        [HttpPost]
        public ActionResult Verification_usermail(Member member, string verify_code)
        {

            if (Session["Verificationcode"].Equals(verify_code) == false)
            {
                ViewBag.verify_code = "驗證碼錯誤!!";
            }
            else { 

            if (ModelState.IsValid)
            {
                member.Verify_status = true;
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index", "Home");
                return Content("<script>alert('註冊成功!! 請重新登入');window.location.href='/Login/Mem_Login';</script>");

                }
            }
             return View(member);

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

            string emailto = Session["usermail"].ToString();
            string subject = "您好" + Session["username"].ToString() + ",感謝您加入寫書人,請驗證您的信箱";
            string body = "您的驗證碼是" + Verificationcode+"<br>執行重新取得驗證碼此組驗證碼將失效";


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

        public JsonResult check_Nickname(string Nickname)
        {
            return Json(!db.Member.Any(x => x.Nickname.ToLower() == Nickname.ToLower()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult check_M_account(string Account)
        {
            return Json(!db.Member.Any(x => x.Account.ToLower() == Account.ToLower()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult check_M_email(string Email)
        {
            return Json(!db.Member.Any(x => x.Email.ToLower() == Email.ToLower()), JsonRequestBehavior.AllowGet);
        }
    }
}