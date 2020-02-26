using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using OriginalWorldProject.App_Code;
namespace OriginalWorldProject.Controllers
{
    public class testController : Controller
    {
        // GET: test
        public ActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Index(string usermail, string username)
        {
            SendEmail(usermail, username);
            return View();
        }

        public void SendEmail(string usermail,string username) {

            RandomPassword ver_code = new RandomPassword();
            string Verificationcode = ver_code.RandomVerificationcode();

            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            bool enableSSL = true;
            string emailFrom = "im.writter0221@gmail.com";
            string password = "asd7821778@";

            string emailto = usermail;
            string subject = "您好"+ username + ",感謝您加入寫書人,請驗證您的信箱";
            string body="您的驗證碼是"+ Verificationcode;


            using (MailMessage mail = new MailMessage()) {
                mail.From = new MailAddress(emailFrom,"寫書人",System.Text.Encoding.UTF8);
                mail.To.Add(emailto);
                mail.Subject = subject;
                mail.Body = body;

                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber)) {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
        }
    }
}