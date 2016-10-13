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
using System.IO;
using PagedList.EntityFramework;
using LearningManagementSystem.Infastructure;

namespace LearningManagementSystem.Controllers
{
    [Authorize]
    [SelectedTab("gallery")]
    public class GallerieController : BaseSecurityController
    {
        [AllowAnonymous]
        public async Task<ActionResult> Index(int? page)
        {
            var list = await db.Gallery.OrderByDescending(i => i.GalleryID).ToPagedListAsync(page ?? 1, 24);
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IEnumerable<HttpPostedFileBase> Image)
        {
            foreach (var item in Image)
            {
                var name = Path.GetFileName(item.FileName);
                var extension = Path.GetExtension(name).ToLower();
                if (extension == ".png" || extension == ".jpg" || extension == ".tif" || extension == ".gif" || extension == ".gpeg")
                {
                    var path = System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("~/Content/UploadImages/", name));
                    item.SaveAs(path);
                    var add = new Gallery
                    {
                        ImageName = name,
                        ImagePath = "~/Content/UploadImages/" + name
                    };
                    db.Gallery.Add(add);
                }
                else
                {
                    ModelState.AddModelError("", "Image type should be gif, jpeg, jpg, tif, png");
                    return View();
                }
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
