using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OriginalWorldProject.Models;
using OriginalWorldProject.App_Code;

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
        public ActionResult Signup(string nickname,string account,string m_password,string phone,string email,DateTime birthday,Boolean gender,string confirm_pwd)
        {


            string id = db.Member.OrderByDescending(m => m.MemberID).Select(m => m.MemberID).FirstOrDefault();
            string nickname_vaild = db.Member.Where(m => m.Nickname==nickname).Select(m=>m.Nickname).FirstOrDefault();
            string account_vaild = db.Member.Where(m => m.Account==account).Select(m => m.Account).FirstOrDefault();
            string phone_vaild = db.Member.Where(m => m.Phone==phone).Select(m => m.Phone).FirstOrDefault();
            string email_vaild = db.Member.Where(m => m.Email==email).Select(m => m.Email).FirstOrDefault();

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
                if (nickname_vaild == null && account_vaild == null && phone_vaild == null && email_vaild == null && confirm_pwd == m_password)
                {
                    member.MemberID = new_ID;
                    member.Nickname = nickname;
                    member.Account = account;
                    member.M_Password = m_password;
                    member.Phone = phone;
                    member.Email = email;
                    member.Gender = gender;
                    member.Birthday = birthday;
                    member.M_status = false;
                    member.Writter_qualifications = false;
                    member.Confirm_pwd = confirm_pwd;

                    db.Member.Add(member);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Members");
                }
            }
                if(nickname_vaild != null || account_vaild != null || phone_vaild != null || email_vaild != null || confirm_pwd != m_password){ 
                    if (nickname_vaild != null) ViewBag.Nickname = "名稱已被使用!!";
                    if (account_vaild != null) ViewBag.Account = "帳號已被註冊!!";
                    if (phone_vaild != null) ViewBag.Phone = "電話已被註冊!!";
                    if (email_vaild != null) ViewBag.Email = "信箱已被註冊!!";
                    if (confirm_pwd != m_password) ViewBag.Confirm_pwd = "密碼不相符!!";
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
        public JsonResult check_M_email(string Email)
        {
            return Json(!db.Member.Any(x => x.Email.ToLower() == Email.ToLower()), JsonRequestBehavior.AllowGet);
        }
        public JsonResult check_M_Phone(string Phone)
        {
            return Json(!db.Member.Any(x => x.Phone.ToLower() == Phone.ToLower()), JsonRequestBehavior.AllowGet);
        }
    }
}