using LearningManagementSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using System.Web;
using System.IO;
using LearningManagementSystem.Data;
using LearningManagementSystem.ViewModels;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Twilio;
using System.Collections.Generic;
using System.Data;
using PagedList.EntityFramework;
using System.Net;

namespace LearningManagementSystem.Controllers
{
    [Authorize]
    public class AdminController : BaseSecurityController
    {

        [Authorize(Roles = "admin")]
        public ActionResult GetAllUsers(int? page)
        {
            //var user = from u in db.Users
            //           select new
            //           {
            //               u.Id,
            //               u.UserName,
            //               Roles = db.Roles.Where(r => u.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name)
            //};
            //var user = from u in db.Users
            //           from ur in u.Roles
            //           join r in db.Roles on ur.RoleId equals r.Id
            //           select new UsersViewModel
            //           {
            //               Id = u.Id,
            //               Email = u.UserName,
            //               Name = u.FirstName + " " + u.LastName,
            //               Role = r.Name,
            //           };

            //var list = user.OrderBy(u => u.Name).ToPagedList(page ?? 1, 20);
            return View();
        }
        [Authorize(Roles = "admin")]
        public ActionResult AjaxGetAllUsers(int? page, string search, string sortBy)
        {
            ViewBag.SortByName = string.IsNullOrEmpty(sortBy) ? "Name Decs" : "";
            ViewBag.SortByRole = sortBy == "Role" ? "Role Desc" : "Role";
            if (search == "")
            {
                search = null;
            }
            var user = from u in db.Users
                       from ur in u.Roles
                       join r in db.Roles on ur.RoleId equals r.Id
                       select new UsersViewModel
                       {
                           Id = u.Id,
                           Email = u.UserName,
                           Name = u.FirstName + " " + u.LastName,
                           Role = r.Name,
                           Phone = u.PhoneNumber
                       };
            var li = user.AsQueryable();
            if (search != null)
            {
                li = li.Where(a => a.Name.ToLower().Contains(search.ToLower()));
            }
            else
            {
                li = user;
            }

            switch (sortBy)
            {
                case "Role Desc":
                    li = li.OrderByDescending(a => a.Role);
                    break;
                case "Name Decs":
                    li = li.OrderByDescending(a => a.Name);
                    break;
                case "Role":
                    li = li.OrderBy(a => a.Role);
                    break;
                default:
                    li = li.OrderBy(a => a.Name);
                    break;
            }
            var list = li.ToPagedList(page ?? 1, 10);
            return PartialView("_PartialUser", list);

            //var list = user.OrderBy(u => u.Name).ToPagedList(page ?? 1, 10);
            //return PartialView("_PartialUser", list);
        }
        [HttpGet, Authorize(Roles = "admin")]
        public ActionResult RegisterAdmin()
        {
            return View();
        }
        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult> RegisterAdmin(RegisterAdmin model, string role)
        {
            model.Role = role;
            if (ModelState.IsValid)
            {
                var find = await UserManager.FindByEmailAsync(model.Email);
                if (find == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        //PhoneNumber = model.ContactNumber,
                        EmailConfirmed = true
                    };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(user.Id, role);
                        return RedirectToAction("GetAllUsers");
                    }
                    else
                    {
                        AddErrors(result);
                        return View();
                    }
                }
                ModelState.AddModelError("", "User Already Exists");
                return View();
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                ModelState.AddModelError("", "User Already Exists");
                return View(model);
            }
        }
        [Authorize(Roles = "admin")]
        public ActionResult DashBoard()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        public ActionResult AdminNameGet()
        {
            InstructorViewModels stuvm = new InstructorViewModels();
            var user = User.Identity.GetUserId();
            var find = db.Users.FirstOrDefault(st => st.Id == user);
            stuvm.FirstName = find.FirstName;
            stuvm.LastName = find.LastName;
            return PartialView("_PartialAdminHeader", stuvm);
        }





        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var role = await UserManager.GetRolesAsync(id);
            var user = await UserManager.FindByIdAsync(id);

            if (role.Contains("student"))
            {
                Student stu = db.Students.Where(u => u.UserId == id).FirstOrDefault();
                if (stu.Image != null)
                {
                    var delpath = Path.Combine(Server.MapPath(stu.Image));
                    System.IO.File.Delete(delpath);
                }
                db.Students.Remove(stu);
                db.SaveChanges();
                if (user != null)
                {
                    var login = user.Logins.ToList();
                    foreach (var log in login)
                    {
                        await UserManager.RemoveLoginAsync(log.UserId, new UserLoginInfo(log.LoginProvider, log.ProviderKey));
                    }
                    if (role.Count() > 0)
                    {
                        foreach (var item in role.ToList())
                        {
                            var getrole = await UserManager.RemoveFromRolesAsync(user.Id, item);
                        }
                    }
                    await UserManager.DeleteAsync(user);
                }
            }

            else if (role.Contains("instructor"))
            {
                Instructor ins = db.Instrouctor.Where(u => u.UserId == id).FirstOrDefault();
                if (ins.Image != null)
                {
                    var delpath = Path.Combine(Server.MapPath(ins.Image));
                    System.IO.File.Delete(delpath);
                }
                db.Instrouctor.Remove(ins);
                db.SaveChanges();
                if (user != null)
                {
                    var login = user.Logins.ToList();
                    foreach (var log in login)
                    {
                        await UserManager.RemoveLoginAsync(log.UserId, new UserLoginInfo(log.LoginProvider, log.ProviderKey));
                    }
                    if (role.Count() > 0)
                    {
                        foreach (var item in role.ToList())
                        {
                            var getrole = await UserManager.RemoveFromRolesAsync(user.Id, item);
                        }
                    }
                    await UserManager.DeleteAsync(user);
                }
            }
            else if (role.Contains("admin"))
            {
                if (user != null)
                {
                    var login = user.Logins.ToList();
                    foreach (var log in login)
                    {
                        await UserManager.RemoveLoginAsync(log.UserId, new UserLoginInfo(log.LoginProvider, log.ProviderKey));
                    }
                    if (role.Count() > 0)
                    {
                        foreach (var item in role.ToList())
                        {
                            var getrole = await UserManager.RemoveFromRolesAsync(user.Id, item);
                        }
                    }
                    await UserManager.DeleteAsync(user);
                }
            }

            return RedirectToAction("GetAllUsers");
        }

        public FileResult Download(string filename)
        {
            //return
            var filepath = Path.Combine(Server.MapPath("~/Content/FileUpload/"), filename);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), filename);
        }



        [HttpGet, Authorize(Roles = "admin")]
        public ActionResult EnrolManage(int? page)
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult AjaxPaging(int? page)
        {
            var list = db.tempenrollments.Where(en => en.IsApproved == "Pending")
                .OrderByDescending(en => en.EnrollmentID).ToList().ToPagedList(page ?? 1, 10);
            return PartialView("_EnrolPartial", list);
        }

        [Authorize(Roles = "admin")]
        public ActionResult EnrolManage(IEnumerable<int> CheckIds, int? page)
        {
            if (CheckIds == null)
            {
                ModelState.AddModelError("", "Select any enrollment request to approve");
                var list = db.tempenrollments.Where(en => en.IsApproved == "Pending")
                                .OrderByDescending(en => en.EnrollmentID).ToList().ToPagedList(page ?? 1, 10);
                return View(list);
            }
            try
            {
                var items = db.tempenrollments.Where(en => CheckIds.Contains(en.EnrollmentID)).ToList();
                db.tempenrollments.Where(en => CheckIds.Contains(en.EnrollmentID)).ToList().ForEach(x => x.IsApproved = "Approved");
                db.SaveChanges();
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to enroll in current course.");
            }
            return RedirectToAction("EnrolManage", "Admin");
        }



        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public async Task<ActionResult> Redirecttoprofile(string id)
        {
            var check = await db.Students.Where(a => a.UserId == id).FirstOrDefaultAsync();
            if (check != null)
            {
                return RedirectToAction("StudentProfile", "Students", new { id = id });
            }
            else
            {
                return RedirectToAction("InstructorPublicProfile", "Instructors", new { id = id });
            }
        }



        [Authorize(Roles = "admin"), HttpGet]
        public ActionResult SendSMS()
        {
            return View();
        }
        [Authorize(Roles = "admin"), HttpPost]
        public ActionResult SendSMS(SMSViewModels model)
        {
            string AccountSid = "AC75334218590f1c58fc62ae935514d9e0";
            string AuthToken = "015be0af50efa89c35f6e2d036ab115d";
            var abc = new TwilioRestClient(AccountSid, AuthToken);

            try
            {
                var message = abc.SendMessage("+12056831033", model.Number, model.Message);
                ViewBag.Message = "Message has been sent successfully";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }




        public async Task<ActionResult> ImageList(int? page)
        {
            var list = await db.Gallery.OrderBy(o => o.GalleryID).ToPagedListAsync(page ?? 1, 20);
            return View(list);
        }
        public async Task<ActionResult> DeleteImage(int? id)
        {
            Gallery gallery = await db.Gallery.FindAsync(id);
            try
            {
                var path = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("~/Content/UploadImages/", gallery.ImageName));
                System.IO.File.Delete(path);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("ImageList");
            }
            db.Gallery.Remove(gallery);
            await db.SaveChangesAsync();
            return RedirectToAction("ImageList");
        }


        public async Task<ActionResult> BatchList(int? page)
        {
            var list = await db.batch.OrderByDescending(b => b.BatchID).ToPagedListAsync(page ?? 1, 20);
            return View(list);
        }
        public async Task<ActionResult> DelBatches(int? id)
        {
            var assignment = await db.Assignemt.Where(i => i.BatchID == id).ToListAsync();
            var lecture = await db.Lecture.Where(i => i.BatchID == id).ToListAsync();
            var attendance = await db.attendance.Where(i => i.BatchID == id).ToListAsync();
            var comment = await db.Comment.Where(i => i.BatchID == id).ToListAsync();
            var payment = await db.Payment.Where(i => i.PaymentID == id).ToListAsync();
            var enrolemt = await db.tempenrollments.Where(i => i.BatchID == id).ToListAsync();
            var batc =await db.batch.Where(i => i.BatchID == id).FirstOrDefaultAsync();
            if (assignment.Count > 0)
            {
                foreach (var item in assignment)
                {
                    var path = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("~/Content/FileUpload/", item.FileName));
                    System.IO.File.Delete(path);
                    db.Assignemt.Remove(item);
                }
            }
            if (lecture.Count > 0)
            {
                foreach (var item in lecture)
                {
                    var path = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("~/Content/FileUpload/", item.FileName));
                    System.IO.File.Delete(path);
                    db.Lecture.Remove(item);
                }
            }
            if (attendance.Count > 0)
            {
                foreach (var item in attendance)
                {
                    db.attendance.Remove(item);
                }
            }
            if (comment.Count > 0)
            {
                foreach (var item in comment)
                {
                    db.Comment.Remove(item);
                }
            }
            if (payment.Count > 0)
            {
                foreach (var item in payment)
                {
                    db.Payment.Remove(item);
                }
            }
            if (enrolemt.Count > 0)
            {
                foreach (var item in enrolemt)
                {
                    db.tempenrollments.Remove(item);
                }
            }
            db.batch.Remove(batc);
            await db.SaveChangesAsync();
            return RedirectToAction("BatchList");
        }



        public async Task<ActionResult> EditEmail(string id)
        {
            var user = await db.Users.Where(i => i.Id == id).FirstOrDefaultAsync();
            var u = new EmailUpdateViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return View(u);
        }
        [HttpPost]
        public async Task<ActionResult> EditEmail(EmailUpdateViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            var u = new ApplicationUser
            {
                Id = model.Id,
                AccessFailedCount = user.AccessFailedCount,
                Email = model.Email,
                UserName = model.Email,
                EmailConfirmed = user.EmailConfirmed,
                FirstName = model.FirstName,
                LastName = model.LastName,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEndDateUtc = user.LockoutEndDateUtc,
                PasswordHash = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                SecurityStamp = user.SecurityStamp,
                TwoFactorEnabled = user.TwoFactorEnabled,
            };
            //await UserManager.UpdateAsync(user);
            db.Entry(u).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("GetAllUsers");
        }
    }
}