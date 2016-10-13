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
using PagedList;

namespace LearningManagementSystem.Controllers
{
    [Authorize]
    public class PaymentsController : BaseSecurityController
    {

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxPayments(int? page)
        {
            var payments = db.Payment.Include(p => p.Batch);
            return PartialView("_PartialPayments", payments.OrderByDescending(o => o.PaymentID).ToPagedList(page ?? 1, 10));
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.PaymentID = new SelectList(db.batch, "BatchID", "BatchName");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "admin")]
        public async Task<ActionResult> Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                var name = await db.batch.Where(b => b.BatchID == payment.PaymentID).FirstOrDefaultAsync();
                payment.PaymentForBatch = name.BatchName;
                var check = await db.Payment.Where(p => p.PaymentForBatch == payment.PaymentForBatch).FirstOrDefaultAsync();
                if (check != null)
                {
                    ViewBag.PaymentID = new SelectList(db.batch, "BatchID", "BatchName", payment.PaymentID);
                    ModelState.AddModelError("", "Payment Already Exists for this batch ...!!!");
                    return View(payment);
                }
                else
                {
                    payment.PaymentDate = DateTime.Now.Date;
                    db.Payment.Add(payment);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.PaymentID = new SelectList(db.batch, "BatchID", "BatchName", payment.PaymentID);
            return View(payment);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payment.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentID = new SelectList(db.batch, "BatchID", "BatchName", payment.PaymentID);
            return View(payment);
        }
        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentID = new SelectList(db.batch, "BatchID", "BatchName", payment.PaymentID);
            return View(payment);
        }


        [Authorize(Roles = "instructor")]
        public async Task<ActionResult> InstPaymentView()
        {
            var user = User.Identity.GetUserId();
            var list = await db.Instrouctor.Where(i => i.UserId == user).FirstOrDefaultAsync();
            var name = list.FirstName + " " + list.LastName;
            var findInstBatch = await db.batch.Where(b => b.InstructorNamme == name).ToListAsync();
            return View(findInstBatch);
        }




        public ActionResult StudentFeeList()
        {
            return View();
        }

        public ActionResult AjaxFeesList(string search, int? page)
        {
            if (search != null)
            {
                var list = db.Fee.OrderBy(o => o.StudentFeeID).Where(s => s.Enrollment.Student.FirstName.Contains(search)).ToPagedList(page ?? 1, 20);
                return PartialView("_AjaxFeesList", list);
            }
            else
            {
                var list = db.Fee.OrderBy(o => o.StudentFeeID).ToPagedList(page ?? 1, 20);
                return PartialView("_AjaxFeesList", list);
            }
        }

        [Authorize(Roles = "admin"), HttpGet]
        public async Task<ActionResult> StudentFeeCreate()
        {
            ViewBag.EnrollList = await db.tempenrollments.OrderBy(i => i.EnrollmentID).GroupBy(g=>g.Batch.BatchName).
                Select(s => new SelectListItem { Value = s.FirstOrDefault().BatchID.ToString(), Text = s.FirstOrDefault().Batch.BatchName }).ToListAsync();
            return View();
        }
        public ActionResult EnrollStudent(int Batch)
        {
            ViewBag.StudentList = db.tempenrollments.OrderBy(o => o.EnrollmentID).Where(i => i.BatchID == Batch)
                .Select(s => new SelectListItem { Value = s.EnrollmentID.ToString(), Text = s.Student.FirstName + " " + s.Student.LastName }).ToList();
            return PartialView("_StudentList");
        }
        [HttpPost]
        public async Task<ActionResult> StudentFeeCreate(StudentFee model, int Enrollment)
        {
            var check = await db.Fee.Where(i => i.EnrollmentID == Enrollment).FirstOrDefaultAsync();
            if (check == null)
            {
                model.EnrollmentID = Enrollment;
                db.Fee.Add(model);
                await db.SaveChangesAsync();
                return Content("Payment has been added Successfully");
            }
            else
            {
                return Content("Payment already exists for this student for this course");
            }
        }


        [HttpGet]
        public async Task<ActionResult> FeesEdit(int? id)
        {
            var find = await db.Fee.Where(i => i.StudentFeeID == id).FirstOrDefaultAsync();
            return View(find);
        }
        [HttpPost]
        public async Task<ActionResult> FeesEdit(StudentFee model)
        {
            db.Entry(model).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("StudentFeeList");
        }
    }
}
