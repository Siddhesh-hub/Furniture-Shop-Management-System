using Fitments_2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Fitments_2.Controllers
{
    public class RegisterController : Controller
    {
        Fitments_2Entities db = new Fitments_2Entities();
            // GET: Register
        public ActionResult Login()
        {
            return View();
        }
        public JsonResult SaveData(Tbl_SiteUser model)
        {
            model.IsValid = false;
            db.Tbl_SiteUser.Add(model);
            db.SaveChanges();
            BuildEmailTemplate(model.ID);

            return Json("Registration Successfull",JsonRequestBehavior.AllowGet);
        }

        public ActionResult Confirm(int regId)
        {
            ViewBag.regID = regId;
            return View();
        }

        public JsonResult RegisterConfirm(int regId)
        {
            Tbl_SiteUser Data = db.Tbl_SiteUser.Where(x => x.ID == regId).FirstOrDefault();
            Data.IsValid = true;
            db.SaveChanges();
            var msg = "Your Email is Verified!";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public void BuildEmailTemplate(int regID)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" +".cshtml");
            var regInfo = db.Tbl_SiteUser.Where(x => x.ID == regID).FirstOrDefault();
            var url = "https://localhost:44326/"+"Register/Confirm?regId="+regID;
            body = body.Replace("@ViewBag.ConfirmationLink", url) ;
            body = body.ToString();
            BuildEmailTemplate("Your Account is Successfully created", body, regInfo.Email);

        }

        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo )
        {
            string from, to, bcc, cc, subject, body;
            from = "siddhesh.s.pansare@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!String.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!String.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);


        }

        public static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("siddhesh.s.pansare@gmail.com", "Nutan@2016");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult CheckValidUser(Tbl_SiteUser model)
        {
            string result = "Fail";
            var DataItem = db.Tbl_SiteUser.Where(x => x.Email == model.Email && x.Password == model.Password).SingleOrDefault();
            if (DataItem != null)
            {
                Session["UserID"] = DataItem.ID.ToString();
                Session["UserName"] = DataItem.Username.ToString();
                result = "Success";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AfterLogin()
        {
            if (Session["UserID"] != null)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}
