using LearningManagementSystem.Data;
using LearningManagementSystem.Infastructure;
using LearningManagementSystem.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Authorize]
    public class PagesController : BaseSecurityController
    {
        // GET: Home
        [AllowAnonymous]
        [SelectedTab("home")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DivertTo()
        {
            if (User.IsInRole("student"))
            {
                return RedirectToAction("DashBoard", "Students");
            }
            else if (User.IsInRole("instructor"))
            {
                return RedirectToAction("DashBoard", "Instructors");
            }
            else if (User.IsInRole("admin"))
            {
                return RedirectToAction("DashBoard", "Admin");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [ChildActionOnly]
        public ActionResult studentdropdown()
        {

            var user = User.Identity.GetUserId();
            var find = db.Students.Where(s => s.UserId == user).FirstOrDefault();
            return PartialView("_PartialStudent", find);
        }
        [ChildActionOnly]
        public ActionResult instructordropdown()
        {

            var user = User.Identity.GetUserId();
            var find = db.Instrouctor.Where(s => s.UserId == user).FirstOrDefault();
            return PartialView("_PartialInstructor", find);
        }
        [ChildActionOnly]
        public ActionResult admin()
        {
            var user = User.Identity.GetUserId();
            var find = db.Users.Where(s => s.Id == user).FirstOrDefault();
            return PartialView("_PartialAdmin", find);
        }

        [HttpGet, AllowAnonymous]
        [SelectedTab("contact")]
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> ContactUs(ContactUsViewModels contact)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p> <p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("info@microtechx.org"));
                message.From = new MailAddress(contact.Email);
                message.Subject = contact.Subject;
                message.Body = string.Format(body, contact.Name, contact.Email, contact.Message);
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "no_reply@microtechx.org",
                        Password = "Mcse2009$"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "mail.microtechx.org";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult ListCourses()
        {
                var list = db.batch.OrderByDescending(o => o.BatchID).ToList().Take(6);
                return PartialView("_ListCourses", list);
        }
    }
}