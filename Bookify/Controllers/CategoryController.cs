using Bookify.Data;
using Bookify.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context;

        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            List<Category> categories = context.Categories.ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View(); 
        }

        [HttpPost]
        public IActionResult Create(Category categoryRequest)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(categoryRequest);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(categoryRequest);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category categoryRequest)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Update(categoryRequest);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(categoryRequest);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost , ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? category = context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            context.Categories.Remove(category);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
