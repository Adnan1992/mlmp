using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LearningManagementSystem.Data;
using LearningManagementSystem.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace LearningManagementSystem.Controllers
{
    [Authorize(Roles = "instructor")]
    public class AssignmetsController : BaseSecurityController
    {
        public async Task<ActionResult> Upload()
        {
            var user = User.Identity.GetUserId();
            var ins = await db.Instrouctor.Where(f => f.UserId == user).FirstOrDefaultAsync();
            var Name = ins.FirstName + " " + ins.LastName;
            ViewBag.Batch = await db.batch.OrderByDescending(a => a.BatchID).Where(b => b.InstructorNamme == Name).Select(l => new SelectListItem { Value = l.BatchID.ToString(), Text = l.BatchName }).ToListAsync();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(Assignmet assignmet, HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    ModelState.AddModelError("", "file is requires");
                    var user = User.Identity.GetUserId();
                    var ins = await db.Instrouctor.Where(f => f.UserId == user).FirstOrDefaultAsync();
                    var Name = ins.FirstName + " " + ins.LastName;
                    ViewBag.Batch = await db.batch.OrderByDescending(a => a.BatchID).Where(b => b.InstructorNamme == Name).Select(l => new SelectListItem { Value = l.BatchID.ToString(), Text = l.BatchName }).ToListAsync();
                    return View(assignmet);
                }
                if (file.ContentLength > 10 * 1024 * 1024)
                {
                    ModelState.AddModelError("", "Max length of file will be 10 mb");
                    var user = User.Identity.GetUserId();
                    var ins = await db.Instrouctor.Where(f => f.UserId == user).FirstOrDefaultAsync();
                    var Name = ins.FirstName + " " + ins.LastName;
                    ViewBag.Batch = await db.batch.OrderByDescending(a => a.BatchID).Where(b => b.InstructorNamme == Name).Select(l => new SelectListItem { Value = l.BatchID.ToString(), Text = l.BatchName }).ToListAsync();
                    return View(assignmet);
                }
                var filename = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(filename).ToLower();
                if (extension == ".png" || extension == ".jpg" || extension == ".png" || extension == ".tif" || extension == ".gif"
                    || extension == ".gpeg" || extension == ".docx" || extension == ".doc" || extension == ".rar"
                    || extension == ".zip" || extension == ".pptx" || extension == ".pdf")
                {
                    var path = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("~/Content/FileUpload/", filename));
                    file.SaveAs(path);
                    assignmet.FileName = filename;
                    assignmet.Path = "~/Content/FileUpload/" + filename;
                    db.Assignemt.Add(assignmet);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Details", "Courses", new { id = assignmet.BatchID });
                }
                else
                {
                    ModelState.AddModelError("", "File format is not valid");
                    var user = User.Identity.GetUserId();
                    var ins = await db.Instrouctor.Where(f => f.UserId == user).FirstOrDefaultAsync();
                    var Name = ins.FirstName + " " + ins.LastName;
                    ViewBag.Batch = await db.batch.OrderByDescending(a => a.BatchID).Where(b => b.InstructorNamme == Name).Select(l => new SelectListItem { Value = l.BatchID.ToString(), Text = l.BatchName }).ToListAsync();
                    return View(assignmet);
                }
            }
            else
            {
                var user = User.Identity.GetUserId();
                var ins = await db.Instrouctor.Where(f => f.UserId == user).FirstOrDefaultAsync();
                var Name = ins.FirstName + " " + ins.LastName;
                ViewBag.Batch = await db.batch.OrderByDescending(a => a.BatchID).Where(b => b.InstructorNamme == Name).Select(l => new SelectListItem { Value = l.BatchID.ToString(), Text = l.BatchName }).ToListAsync();
                return View(assignmet);
            }
        }

        public async Task<ActionResult> Delete(int? id)
        {
            var f = await db.Assignemt.Where(a => a.AssignmetID == id).FirstOrDefaultAsync();
            if (f.FileName != null)
            {
                var delpath = Path.Combine(Server.MapPath(f.Path));
                System.IO.File.Delete(delpath);
            }
            db.Assignemt.Remove(f);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Courses", new { id = f.BatchID });
        }






        public async Task<ActionResult> UploadLecture()
        {
            var user = User.Identity.GetUserId();
            var ins = await db.Instrouctor.Where(f => f.UserId == user).FirstOrDefaultAsync();
            var Name = ins.FirstName + " " + ins.LastName;
            ViewBag.Batch = await db.batch.OrderByDescending(a => a.BatchID).Where(b => b.InstructorNamme == Name).Select(l => new SelectListItem { Value = l.BatchID.ToString(), Text = l.BatchName }).ToListAsync();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadLecture(Lectures lecture, HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    ModelState.AddModelError("", "file is requires");
                    var user = User.Identity.GetUserId();
                    var ins = await db.Instrouctor.Where(f => f.UserId == user).FirstOrDefaultAsync();
                    var Name = ins.FirstName + " " + ins.LastName;
                    ViewBag.Batch = await db.batch.OrderByDescending(a => a.BatchID).Where(b => b.InstructorNamme == Name).Select(l => new SelectListItem { Value = l.BatchID.ToString(), Text = l.BatchName }).ToListAsync();
                    return View(lecture);
                }
                if (file.ContentLength > 10 * 1024 * 1024)
                {
                    ModelState.AddModelError("", "Max length of file will be 10 mb");
                    var user = User.Identity.GetUserId();
                    var ins = await db.Instrouctor.Where(f => f.UserId == user).FirstOrDefaultAsync();
                    var Name = ins.FirstName + " " + ins.LastName;
                    ViewBag.Batch = await db.batch.OrderByDescending(a => a.BatchID).Where(b => b.InstructorNamme == Name).Select(l => new SelectListItem { Value = l.BatchID.ToString(), Text = l.BatchName }).ToListAsync();
                    return View(lecture);
                }
                var filename = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(filename).ToLower();
                if (extension == ".png" || extension == ".jpg" || extension == ".png" || extension == ".tif" || extension == ".gif"
                    || extension == ".gpeg" || extension == ".docx" || extension == ".doc" || extension == ".rar"
                    || extension == ".zip" || extension == ".pptx" || extension == ".pdf")
                {
                    var path = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("~/Content/FileUpload/", filename));
                    file.SaveAs(path);
                    lecture.FileName = filename;
                    lecture.Path = "~/Content/FileUpload/" + filename;
                    db.Lecture.Add(lecture);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Details", "Courses", new { id = lecture.BatchID });
                }
                else
                {
                    ModelState.AddModelError("", "File format is not valid");
                    var user = User.Identity.GetUserId();
                    var ins = await db.Instrouctor.Where(f => f.UserId == user).FirstOrDefaultAsync();
                    var Name = ins.FirstName + " " + ins.LastName;
                    ViewBag.Batch = await db.batch.OrderByDescending(a => a.BatchID).Where(b => b.InstructorNamme == Name).Select(l => new SelectListItem { Value = l.BatchID.ToString(), Text = l.BatchName }).ToListAsync();
                    return View(lecture);
                }
            }
            else
            {
                var user = User.Identity.GetUserId();
                var ins = await db.Instrouctor.Where(f => f.UserId == user).FirstOrDefaultAsync();
                var Name = ins.FirstName + " " + ins.LastName;
                ViewBag.Batch = await db.batch.OrderByDescending(a => a.BatchID).Where(b => b.InstructorNamme == Name).Select(l => new SelectListItem { Value = l.BatchID.ToString(), Text = l.BatchName }).ToListAsync();
                return View(lecture);
            }
        }

        public async Task<ActionResult> DeleteLecture(int? id)
        {
            var f = await db.Lecture.Where(a => a.LecturesID == id).FirstOrDefaultAsync();
            if (f.FileName != null)
            {
                var delpath = Path.Combine(Server.MapPath(f.Path));
                System.IO.File.Delete(delpath);
            }
            db.Lecture.Remove(f);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Courses", new { id = f.BatchID });
        }


    }
}
