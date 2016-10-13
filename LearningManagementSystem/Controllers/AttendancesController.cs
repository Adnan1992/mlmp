using System.Data;
using System.Linq;
using System.Web.Mvc;
using LearningManagementSystem.Models;
using LearningManagementSystem.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System;

namespace LearningManagementSystem.Controllers
{
    [Authorize(Roles = "instructor")]
    public class AttendancesController : BaseSecurityController
    {

        public ActionResult Index()
        {
            AttendanceViewModels obj = new AttendanceViewModels();

            ViewBag.courselist = db.courses.OrderBy(c => c.CourseID).Select(c => new SelectListItem { Value = c.CourseID.ToString(), Text = c.Title }).ToList();
            return View(obj);
        }
        public ActionResult SelCourse(int Course)
        {
            var find = User.Identity.GetUserId();
            var ins = db.Instrouctor.Where(u => u.UserId == find).FirstOrDefault();
            if (ins != null)
            {
                ViewBag.batchlist = db.batch.OrderBy(b => b.BatchID).Where(b => b.CourseID == Course && b.InstructorNamme == ins.FullName).Select(c => new SelectListItem { Value = c.BatchID.ToString(), Text = c.BatchName }).ToList();
            }
            else
            {
                ViewBag.batchlist = null;
            }
            return PartialView("~/Views/Attendances/markattendance/_selectbatch.cshtml");
        }
        public ActionResult SelectBatch(int Batch)
        {
            //ViewBag.selectsession = db.attendance.OrderBy(c => c.AttendanceID).
            //    Select(c => new SelectListItem { Value = c.AttendanceID.ToString(), Text = c.AttendanceDate.ToString()}).Distinct().ToList();
            ViewBag.selectsession = db.attendance.Where(s => s.BatchID == Batch).GroupBy(a => a.AttendanceDate).Select(a => a.FirstOrDefault().AttendanceDate.ToString()).ToList();
            //  var students = db.attendance.Where(s => s.BatchID == Batch).Select(e=>e.AttendanceDate).ToList();
            return PartialView("~/Views/Attendances/markattendance/_selectSession.cshtml");
        }
        public ActionResult SelectSession(string Session)
        {
            var find = db.attendance.ToList();
            var date = Convert.ToDateTime(Session);
            var check = find.Where(a => a.AttendanceDate.ToString("d") == date.ToString("d")).ToList();
            return PartialView("~/Views/Attendances/markattendance/_markattendance.cshtml", check);
        }
        [HttpPost]
        public ActionResult MarkAttendance(IEnumerable<int> CheckIds)
        {
            try
            {
                db.attendance.Where(a => CheckIds.Contains(a.AttendanceID)).ToList().ForEach(a => a.AttendanceStatus = "P");
                db.SaveChanges();
                return RedirectToAction("index");
            }
            catch
            {
                ModelState.AddModelError("", "Some thing Wend Wrong");
                return RedirectToAction("index");
            }

        }




        [HttpGet]
        public ActionResult CreateSession()
        {
            ViewBag.courselist = db.courses.OrderBy(c => c.CourseID).Select(c => new SelectListItem { Value = c.CourseID.ToString(), Text = c.Title }).ToList();
            return View();
        }
        public ActionResult BatchforAttendance(int Course)
        {
            AttendanceViewModels obj = new AttendanceViewModels();
            var find = User.Identity.GetUserId();
            var ins = db.Instrouctor.Where(u => u.UserId == find).FirstOrDefault();
            if (ins != null)
            {
                ViewBag.batchlist = db.batch.OrderBy(b => b.BatchID).Where(b => b.CourseID == Course && b.InstructorNamme == ins.FullName).Select(c => new SelectListItem { Value = c.BatchID.ToString(), Text = c.BatchName }).ToList();
            }
            else
            {
                ViewBag.batchlist = null;
            }
            return PartialView("~/Views/Attendances/createattendance/BatchforAttendance.cshtml", obj);
        }
        [HttpPost]
        public ActionResult CreateSession(AttendanceViewModels model, int Batch)
        {
            try
            {
                Attendance ma;
                var enrollmentid = db.tempenrollments.
                   Where(b => b.BatchID == Batch && b.IsApproved == "Approved").Select(e => e.EnrollmentID).ToList();
                //db.attendance.AsEnumerable().Select(x => new Attendance
                //{

                //});
                foreach (var item in enrollmentid)
                {
                    ma = new Attendance()
                    {
                        AttendanceDate = model.AttendanceDate,
                        // MarkAttendance=false,
                        EnrollmentID = item,
                        BatchID = Batch,
                        AttendanceStatus = "A"
                    };
                    db.attendance.Add(ma);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Error");
                return View();
            }
        }








        [HttpGet]
        public ActionResult GetAttendance()
        {
            ViewBag.courselist = db.courses.OrderBy(c => c.CourseID).Select(c => new SelectListItem { Value = c.CourseID.ToString(), Text = c.Title }).ToList();
            return View();
        }
        public ActionResult getBatch(int Course)
        {
            var find = User.Identity.GetUserId();
            var ins = db.Instrouctor.Where(u => u.UserId == find).FirstOrDefault();
            if (ins != null)
            {
                ViewBag.batchlist = db.batch.OrderBy(b => b.BatchID).Where(b => b.CourseID == Course && b.InstructorNamme == ins.FullName).Select(c => new SelectListItem { Value = c.BatchID.ToString(), Text = c.BatchName }).ToList();
            }
            else
            {
                ViewBag.batchlist = null;
            }
            return PartialView("~/Views/Attendances/getattendance/_getbatch.cshtml");
        }
        public ActionResult getDate(int Batch)
        {
            var find = User.Identity.GetUserId();
            var ins = db.Instrouctor.Where(u => u.UserId == find).FirstOrDefault();
            if (ins != null)
            {
                ViewBag.dateList = db.attendance.OrderBy(a => a.AttendanceID).Where(b => b.BatchID == Batch).GroupBy(a => a.AttendanceDate).Select(d => new SelectListItem { Value = d.FirstOrDefault().AttendanceDate.ToString(), Text = d.FirstOrDefault().AttendanceDate.ToString() }).ToList();
                ViewBag.Id = Batch;
            }
            else
            {
                ViewBag.dateList = null;
            }
            return PartialView("~/Views/Attendances/getattendance/_getdate.cshtml");
        }
        public ActionResult getList(DateTime Date, int Id)

        {
            // obj.ListDate = db.attendance.Where(a => a.BatchID == Batch).GroupBy(a=>a.AttendanceDate).Select(a=>a.FirstOrDefault().AttendanceDate).ToList();
            //obj.ListStudent = db.attendance.Where(a => a.BatchID == Batch).GroupBy(a => a.Enrollment.Student.FirstName).Select(a => a.FirstOrDefault().Enrollment.Student.FirstName).ToList();
            //obj.ListAtt = db.attendance.Where(a => a.BatchID == Batch).Select(a => a.AttendanceStatus).ToList();
            //var list = db.attendance.Where(a => a.BatchID == Batch).ToList().GroupBy(a=> new { a.AttendanceDate,a.Enrollment.Student.FirstName}).Select(a=>new { a.Key.AttendanceDate,a.Key.FirstName}).ToList();
            var list = db.attendance.Where(a => a.AttendanceDate == Date && a.BatchID == Id).ToList();
            return PartialView("~/Views/Attendances/getattendance/_getList.cshtml", list);
        }
    }
}
