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

namespace LearningManagementSystem.Controllers
{
    public class CommentsController : BaseSecurityController
    {
        [AllowAnonymous]
        public ActionResult CommentsList(int? id)
        {
            var find = db.Comment.OrderByDescending(o => o.CommentID).Where(f => f.BatchID == id).ToList();
            return PartialView("_PartialCommentsList", find);
        }

        [HttpPost, ValidateAntiForgeryToken,AllowAnonymous]
        public ActionResult Create(Comment comment)
        {
            var user = User.Identity.GetUserId();
            var find = db.Users.FirstOrDefault(a => a.Id == user);
            comment.MadeBy = find.FirstName + " " + find.LastName;
            db.Comment.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "Courses", new { id = comment.BatchID });
        }
    }
}
