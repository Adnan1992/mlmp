using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LearningManagementSystem.Models;
using LearningManagementSystem.Data;
using LearningManagementSystem.Interfacelasses;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using LearningManagementSystem.ViewModels;
using PagedList;
using System.Data.Entity;
using System.Web;
using System.IO;
using PagedList.EntityFramework;
using LearningManagementSystem.Infastructure;
using System;

namespace LearningManagementSystem.Controllers
{
    public static class StringExtensions
    {
        public static string ToSystemString(this IEnumerable<char> source)
        {
            return new string(source.ToArray());
        }
    }
    [Authorize]
    [SelectedTab("course")]
    public class CoursesController : BaseSecurityController
    {
        private IRepository<Course> _repository = null;

        public CoursesController()
        {
            this._repository = new Repository<Course>();
        }


        [AllowAnonymous]
        public async Task<ActionResult> Index(int? page)
        {

            var course = await db.batch.OrderByDescending(b => b.BatchID).ToPagedListAsync(page ?? 1, 12);
            return View(course);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            EnrollmentViewModels enrolment = new EnrollmentViewModels();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Batch batch = await db.batch.FindAsync(id);
            enrolment.BatchId = batch.BatchID;
            enrolment.Description = HttpUtility.HtmlDecode(batch.Description);
            enrolment.UserId = User.Identity.GetUserId();
            enrolment.StartingDate = batch.StartingDate;
            enrolment.FinishingDate = batch.FinishingDate;
            enrolment.InstName = batch.InstructorNamme;
            enrolment.Path = batch.Course.Path;
            enrolment.Title = batch.Course.Title;
            ViewBag.UserID = enrolment.UserId;
            if (batch == null)
            {
                return HttpNotFound();
            }
            else
            {
                var u = await db.tempenrollments.Where(en => en.UserId == enrolment.UserId && en.BatchID == enrolment.BatchId).CountAsync();

                if (u <= 0)
                {
                    enrolment.Status = false;
                }
                else
                {
                    enrolment.Status = true;
                    var checkStatus = await db.tempenrollments.Where(en => en.UserId == enrolment.UserId && en.BatchID == enrolment.BatchId).Select(en => en.IsApproved).FirstOrDefaultAsync();

                    if (checkStatus == "Approved")
                    {
                        enrolment.CheckStatus = true;
                    }
                    else
                    {
                        enrolment.CheckStatus = false;
                        ViewBag.Message = "Your Request is pending please wait or contact at .............";
                    }
                }
                return View(enrolment);
            }
        }
        [AllowAnonymous]
        public ActionResult Assignments(int? id)
        {
            var list = db.Assignemt.Where(a => a.BatchID == id).ToList();
            return PartialView("_PartialAssignments", list);
        }

        public ActionResult Lectures(int? id)
        {
            var list = db.Lecture.Where(a => a.BatchID == id).ToList();
            return PartialView("_PartialLectures", list);
        }
        [HttpGet, Authorize(Roles = "admin")]
        public ActionResult CreateBatch()
        {
            ViewBag.selectcourse = db.courses.OrderBy(c => c.Title).ToList().Select(cc => new SelectListItem { Value = cc.Title.ToString(), Text = cc.Title }).ToList();
            ViewBag.instructor = db.Instrouctor.OrderBy(c => c.FirstName).ToList().Select(cc => new SelectListItem { Value = cc.FullName.ToString(), Text = cc.FullName }).ToList();
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "admin")]
        public ActionResult CreateBatch(Batch batch, string course, string instructor)
        {
            ViewBag.selectcourse = db.courses.OrderBy(c => c.Title).ToList().Select(cc => new SelectListItem { Value = cc.Title.ToString(), Text = cc.Title }).ToList();
            ViewBag.instructor = db.Instrouctor.OrderBy(c => c.FirstName).ToList().Select(cc => new SelectListItem { Value = cc.FullName.ToString(), Text = cc.FullName }).ToList();
            try
            {
                var result = DateTime.Compare(batch.StartingDate, batch.FinishingDate);
                if(result==0)
                {
                    ModelState.AddModelError("", "Batch starting and Finishing date should not be equal");
                    return View(batch);
                }
                else if(result > 0)
                {
                    ModelState.AddModelError("", "Batch starting date should be earlier than and Finishing date ");
                    return View(batch);
                }
                var findcourse = db.courses.Where(c => c.Title == course).FirstOrDefault();
                if (findcourse != null)
                {
                    batch.CourseID = findcourse.CourseID;
                }
                var date = batch.StartingDate.ToString("MM/dd/yyyy");
                batch.BatchName = "Batch" + "-" + course + "-" + date;
                batch.Title = findcourse.Title;
                batch.Description = findcourse.Description;
                batch.InstructorNamme = instructor;
                var find = db.batch.FirstOrDefault(b => b.BatchName == batch.BatchName);
                if (find == null)
                {
                    db.batch.Add(batch);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.mess = "The Course Already Exists";
                    return View();
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View();

        }


        [Authorize(Roles = "admin")]
        public ActionResult AllCourses()
        {
            var list = db.courses.ToList();
            return View(list);
        }
        [HttpGet, Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "admin")]
        public async Task<ActionResult> Create(Course course, HttpPostedFileBase Image)
        {

            if (ModelState.IsValid)
            {
                if (Image == null)
                {
                    ModelState.AddModelError("", "Image is required");
                    return View(course);
                }
                var find = await db.courses.FirstOrDefaultAsync(c => c.Title.ToLower() == course.Title.ToLower());
                if (find == null)
                {

                    if (Image.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(Image.FileName);
                        var extension = Path.GetExtension(filename).ToLower();
                        if (extension == ".png" || extension == ".jpg" || extension == ".png" || extension == ".tif" || extension == ".gif" || extension == ".gpeg")
                        {
                            var path = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("~/Content/UploadImages/", filename));
                            Image.SaveAs(path);
                            course.ImageName = filename;
                            course.Path = "~/Content/UploadImages/" + filename;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Image type should be gif, jpeg, jpg, tif, png");
                            return View(course);
                        }
                    }
                    var description = HttpUtility.HtmlEncode(course.Description);
                    course.Description = description;
                    db.courses.Add(course);
                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch(Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                        return View(course);
                    }
                    //_repository.Insert(course);
                    //_repository.Save();
                    return RedirectToAction("AllCourses");
                }
                else
                {
                    ViewBag.mess = "The Course Already Exists";
                    return View(course);
                }

            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                ModelState.AddModelError("", messages);
                return View(course);
            }
        }


        [Authorize(Roles = "student")]
        public ActionResult Enrol(int id)
        {

            try
            {
                var users = User.Identity.GetUserId();
                //var find = UserManager.FindById(users);

                var user = db.Students.Where(u => u.UserId == users).FirstOrDefault();
                var add = new Enrollment
                {
                    BatchID = id,
                    StudentID = user.StudentId,
                    IsApproved = "Pending",
                    UserId = users
                };
                db.tempenrollments.Add(add);
                db.SaveChanges();
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to enroll in current course.");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await _repository.GetById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            course.Description = HttpUtility.HtmlDecode(course.Description);
            return View(course);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(Course course, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image == null)
                {
                    var st = new Course
                    {
                        CourseID = course.CourseID,
                        Description = HttpUtility.HtmlEncode(course.Description),
                        ImageName = course.ImageName,
                        Path = course.Path,
                        Title = course.Title
                    };
                    db.Entry(st).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("AllCourses");
                }
                else if (Image.ContentLength > 0)
                {
                    var filename = Path.GetFileName(Image.FileName);
                    var extension = Path.GetExtension(filename).ToLower();
                    if (extension == ".png" || extension == ".jpg" || extension == ".png" || extension == ".tif" || extension == ".gif" || extension == ".gpeg")
                    {
                        if (course.ImageName != null)
                        {
                            var delpath = Path.Combine(Server.MapPath(course.Path));
                            System.IO.File.Delete(delpath);
                        }
                        var path = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("~/Content/UploadImages/", filename));
                        Image.SaveAs(path);
                        course.ImageName = filename;
                        course.Path = "~/Content/UploadImages/" + filename;
                        var c = HttpUtility.HtmlEncode(course.Description);
                        course.Description = c;
                        db.Entry(course).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("AllCourses");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Image type should be gif, jpeg, jpg, tif, png");
                        return View(course);
                    }
                }
            }
            return RedirectToAction("AllCourses");
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Course course = await db.courses.Where(c => c.CourseID == id).FirstOrDefaultAsync();
                if (course.ImageName != null)
                {
                    var delpath = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("~/Content/UploadImages/", course.ImageName));
                    System.IO.File.Delete(delpath);
                }
                db.courses.Remove(course);
                await db.SaveChangesAsync();
            }
            catch (DataException ex)
            {
                //ModelState.AddModelError("", ex.Message);
                //return RedirectToAction("AllCourses");
                return RedirectToAction("AllCourses",
                    new System.Web.Routing.RouteValueDictionary {
                { "id", id },
                { "saveChangesError", true } });
            }
            return RedirectToAction("AllCourses");
        }
    }
}
