using Bookify.DataAccess.Repository.IRepository;
using Bookify.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> productList = unitOfWork.ProductRepository.GetAll().ToList();
            return View(productList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product productFromRequest)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ProductRepository.Add(productFromRequest);
                unitOfWork.Save();
                TempData["success"] = "Product created sucessfully";
                return RedirectToAction("Index");
            }
            return View(productFromRequest);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? product = unitOfWork.ProductRepository.Get(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product productFromRequest)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ProductRepository.Update(productFromRequest);
                unitOfWork.Save();
                TempData["success"] = "Product Updated sucessfully";
                return RedirectToAction("Index");
            }
            return View(productFromRequest);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? product = unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? product = unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            unitOfWork.ProductRepository.Remove(product);
            unitOfWork.Save();
            TempData["success"] = "Product deleted sucessfully";
            return RedirectToAction("Index");
        }

    }
}
