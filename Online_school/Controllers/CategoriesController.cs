using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_school.Data;
using Online_school.Models;

namespace Online_school.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db;

        public CategoriesController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
           
            var categories = from category in db.Categories
                             orderby category.Category_Name
                             select category;
            ViewBag.Categories = categories;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult New(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            else
            {
                return View(cat);
            }
        }

        public IActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(int id, Category requestCategory)
        {
            Category category = db.Categories.Find(id);

            if (ModelState.IsValid)
            {

                category.Category_Name = requestCategory.Category_Name;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(requestCategory);
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Category category = db.Categories.Where(cat => cat.CategoryId == id).First();
            var groups=db.Groups.Where(g => g.CategoryId == id);

            foreach (var group in groups)
            {
                var messages = db.Messages.Where(m => m.GroupId == group.GroupId);
                foreach (var message in messages)
                    db.Messages.Remove(message);
                db.Groups.Remove(group);
            }

            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");  
        }

    }
}
