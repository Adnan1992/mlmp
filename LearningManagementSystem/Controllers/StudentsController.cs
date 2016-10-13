using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LearningManagementSystem.Data;
using LearningManagementSystem.Models;
using LearningManagementSystem.Interfacelasses;
using System.Threading.Tasks;
using LearningManagementSystem.ViewModels;
using Microsoft.AspNet.Identity;
using System.IO;

namespace LearningManagementSystem.Controllers
{
    [Authorize]
    public class StudentsController : BaseSecurityController
    {
        //LMSContext db = new LMSContext();
        private IRepository<Student> _repository = null;

        public StudentsController()
        {
            this._repository = new Repository<Student>();
        }


        [Authorize(Roles = "student")]
        public async Task<ActionResult> Edit(int? id)
        {
            StudentViewModels st = new StudentViewModels();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _repository.GetById(id);
            var user = await db.Users.Where(u => u.Id == student.UserId).FirstOrDefaultAsync();
            st.FirstName = student.FirstName;
            st.LastName = student.LastName;
            st.Gender = student.Gender;
            st.Email = user.Email;
            st.Image = student.Image;
            st.ContactNumber = student.PhoneNumber;
            st.StudentId = student.StudentId;
            st.UserId = student.UserId;
            st.Image = student.Image;

            if (student == null)
            {
                return HttpNotFound();
            }
            return View(st);
        }


        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "student")]
        public async Task<ActionResult> Edit(StudentViewModels student, HttpPostedFileBase Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Image == null)
                    {
                        var st = new Student
                        {
                            StudentId = student.StudentId,
                            FirstName = student.FirstName,
                            LastName = student.LastName,
                            UserId = student.UserId,
                            PhoneNumber = student.ContactNumber,
                            Gender = student.Gender,
                            Image = student.Image,
                            Biography = student.Biography
                        };
                        db.Entry(st).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    else if (Image.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(Image.FileName);
                        var extension = Path.GetExtension(filename).ToLower();
                        if (extension == ".png" || extension == ".jpg" || extension == ".png" || extension == ".tif" || extension == ".gif" || extension == ".gpeg")
                        {
                            if (student.Image != null)
                            {
                                var delpath = Path.Combine(Server.MapPath(student.Image));
                                System.IO.File.Delete(delpath);
                            }
                            var path = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("~/Content/UploadImages/", filename));
                            Image.SaveAs(path);
                            var stt = new Student
                            {
                                StudentId = student.StudentId,
                                FirstName = student.FirstName,
                                LastName = student.LastName,
                                UserId = student.UserId,
                                PhoneNumber = student.ContactNumber,
                                Gender = student.Gender,
                                Image = "~/Content/UploadImages/" + filename,
                                Biography = student.Biography
                            };
                            db.Entry(stt).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Image type should be gif, jpeg, jpg, tif, png");
                            return View(student);
                        }
                    }
                }

                return RedirectToAction("DashBoard");
            }
            catch (DataException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                return View(student);
            }
        }
        [Authorize(Roles = "student")]
        public async Task<ActionResult> MyCourses()
        {
            var user = User.Identity.GetUserId();
            var find = await db.tempenrollments.Where(e => e.UserId == user).ToListAsync();
            return View(find);
        }
        [Authorize(Roles = "student")]
        public async Task<ActionResult> DashBoard()
        {
            var user = User.Identity.GetUserId();
            var ins = await db.Students.Where(i => i.UserId == user).FirstOrDefaultAsync();
            return View(ins);
        }

        [Authorize(Roles = "student")]
        public ActionResult Header()
        {
            StudentViewModels stuvm = new StudentViewModels();
            var user = User.Identity.GetUserId();
            var find = db.Students.FirstOrDefault(st => st.UserId == user);
            stuvm.StudentId = find.StudentId;
            stuvm.FirstName = find.FirstName;
            stuvm.LastName = find.LastName;
            stuvm.Gender = find.Gender;
            stuvm.Image = find.Image;
            return PartialView("_PartialStudentHeader", stuvm);
        }
        [Authorize(Roles = "student")]
        public ActionResult SideBar()
        {
            var user = User.Identity.GetUserId();
            var ins = db.Students.Where(i => i.UserId == user).FirstOrDefault();
            return PartialView("_AccountManageBar", ins);
        }

        [Authorize]
        public async Task<ActionResult> StudentProfile(string id)
        {
            var f = await db.Students.Where(i => i.UserId == id).FirstOrDefaultAsync();
            return View(f);
        }

        [Authorize]
        public async Task<ActionResult> myAttendance()
        {
            var user = User.Identity.GetUserId();
            ViewBag.check = await db.tempenrollments.OrderBy(o => o.EnrollmentID).Where(a => a.UserId == user && a.IsApproved == "Approved")
                .Select(s => new SelectListItem { Value = s.EnrollmentID.ToString(), Text = s.Batch.Course.Title }).ToListAsync();
            return View();
        }

        public ActionResult Atten(int? Enrollment)
        {
            var attendance = db.attendance.Where(i => i.EnrollmentID == Enrollment).ToList();
            return PartialView("attenList", attendance);
        }
    }
}