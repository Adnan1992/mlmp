using LearningManagementSystem.Data;
using LearningManagementSystem.Interfacelasses;
using LearningManagementSystem.Models;
using LearningManagementSystem.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using LearningManagementSystem.Infastructure;

namespace LearningManagementSystem.Controllers
{
    [Authorize]
    [SelectedTab("instructor")]
    public class InstructorsController : BaseSecurityController
    {
        private IRepository<Instructor> _repository = null;
        private IUserManagementRepository userManagement;

        public InstructorsController()
        {
            this._repository = new Repository<Instructor>();
            userManagement = new UserManagementRepository();
        }
        [AllowAnonymous]
        public ActionResult InstList()
        {
            return View(db.Instrouctor.ToList());
        }


        [HttpGet, Authorize(Roles = "admin")]
        public ActionResult RegisterInstructor()
        {
            var roles = ApplicationRoleManager.Create(HttpContext.GetOwinContext());

            if (!roles.RoleExists(SecurityRoles.Admin))
            {
                roles.Create(new IdentityRole { Name = SecurityRoles.Admin });
            }

            if (!roles.RoleExists(SecurityRoles.Instructor))
            {
                roles.Create(new IdentityRole { Name = SecurityRoles.Instructor });
            }

            if (!roles.RoleExists(SecurityRoles.MSA))
            {
                roles.Create(new IdentityRole { Name = SecurityRoles.MSA });
            }
            if (!roles.RoleExists(SecurityRoles.Student))
            {
                roles.Create(new IdentityRole { Name = SecurityRoles.Student });
            }
            ViewBag.role = new SelectList(db.Roles.Select(r => r.Name), "Name");
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "admin")]
        public async Task<ActionResult> RegisterInstructor(EmployeeViewModel model, string role)
        {
            var check = await db.Users.Where(e => e.Email == model.Email).FirstOrDefaultAsync();
            if (check == null)
            {
                try
                {
                    model.Password = Membership.GeneratePassword(15, 7);
                    await userManagement.AddNewInstructor(model, role, model.Password, model.USERID);
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(model.USERID);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = model.USERID, code = code }, protocol: Request.Url.Scheme);
                    var body = "Please confirm your account by clicking <a href =\"" + callbackUrl + "\">here</a> <br>Password for login:   " + model.Password;
                    await UserManager.SendEmailAsync(model.USERID, "Confirm your account", body);
                    return RedirectToAction("GetAllUsers", "Admin");
                }
                catch (Exception ex)
                {
                    ViewBag.role = new SelectList(db.Roles.Select(r => r.Name), "Name");
                    ModelState.AddModelError("", ex.Message);
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "User exists on this email");
                return View(model);
            }

        }


        [HttpGet, Authorize(Roles = "instructor")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = await _repository.GetById(id);
            var user = await db.Users.Where(u => u.Id == instructor.UserId).FirstOrDefaultAsync();
            var ins = new InstructorViewModels
            {
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Gender = instructor.Gender,
                Email = user.Email,
                Image = instructor.Image,
                ContactNumber = instructor.PhoneNumber,
                InstructorId = instructor.InstructorId,
                UserId = instructor.UserId,
                Biography = instructor.Biography,
                Certificates = instructor.Certificates,
                SocialLinks = instructor.SocialLinks
            };
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(ins);
        }

        [HttpPost, Authorize(Roles = "instructor")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(InstructorViewModels instructor, HttpPostedFileBase Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Student st ;
                    if (Image == null)
                    {
                        var st = new Instructor
                        {
                            InstructorId = instructor.InstructorId,
                            FirstName = instructor.FirstName,
                            LastName = instructor.LastName,
                            UserId = instructor.UserId,
                            PhoneNumber = instructor.ContactNumber,
                            Gender = instructor.Gender,
                            Image = instructor.Image,
                            Biography = instructor.Biography,
                            Certificates = instructor.Certificates,
                            SocialLinks = instructor.SocialLinks
                        };
                        db.Entry(st).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    //string ImagePath;
                    else if (Image.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(Image.FileName);
                        var extension = Path.GetExtension(filename).ToLower();
                        if (extension == ".jpg" || extension == ".png" || extension == ".tif" || extension == ".gif" || extension == ".gpeg")
                        {
                            if (instructor.Image != null)
                            {
                                var delpath = Path.Combine(Server.MapPath(instructor.Image));
                                System.IO.File.Delete(delpath);
                            }
                            var path = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("~/Content/UploadImages/", filename));
                            Image.SaveAs(path);
                            var stt = new Instructor
                            {
                                InstructorId = instructor.InstructorId,
                                FirstName = instructor.FirstName,
                                LastName = instructor.LastName,
                                UserId = instructor.UserId,
                                PhoneNumber = instructor.ContactNumber,
                                Gender = instructor.Gender,
                                Image = "~/Content/UploadImages/" + filename,
                                Biography = instructor.Biography,
                                Certificates = instructor.Certificates,
                                SocialLinks = instructor.SocialLinks
                            };
                            db.Entry(stt).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Image type should be gif, jpeg, jpg, tif, png");
                            return View(instructor);
                        }
                    }
                }
                return RedirectToAction("DashBoard");
            }

            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                return View(instructor);
            }
        }

        [Authorize(Roles = "instructor")]
        public async Task<ActionResult> MyCourses(int id)
        {
            //var user = User.Identity.GetUserId();
            var ins = await db.Instrouctor.Where(i => i.InstructorId == id).FirstOrDefaultAsync();
            var find = await db.batch.Where(i => i.InstructorNamme == ins.FullName).ToListAsync();
            return View(find);
        }
        public async Task<ActionResult> DashBoard()
        {
            var user = User.Identity.GetUserId();
            var ins = await db.Instrouctor.Where(i => i.UserId == user).FirstOrDefaultAsync();
            return View(ins);
        }

        public ActionResult Header()
        {
            InstructorViewModels stuvm = new InstructorViewModels();
            var user = User.Identity.GetUserId();
            var find = db.Instrouctor.FirstOrDefault(st => st.UserId == user);
            stuvm.InstructorId = find.InstructorId;
            stuvm.FirstName = find.FirstName;
            stuvm.LastName = find.LastName;
            stuvm.Gender = find.Gender;
            stuvm.Image = find.Image;
            return PartialView("_PartialInstructorHeader", stuvm);
        }
        public ActionResult SideBar()
        {
            var user = User.Identity.GetUserId();
            var ins = db.Instrouctor.Where(i => i.UserId == user).FirstOrDefault();
            return PartialView("_AccountManageBarInstructor", ins);
        }


        [AllowAnonymous]
        public async Task<ActionResult> InstructorPublicProfile(string id)
        {
            var ins = await db.Instrouctor.Where(i => i.UserId == id).FirstOrDefaultAsync();
            return View(ins);
        }
    }
}