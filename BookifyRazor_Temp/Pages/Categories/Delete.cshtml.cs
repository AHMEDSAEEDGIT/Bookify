using BookifyRazor_Temp.Data;
using BookifyRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookifyRazor_Temp.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext context;

        [BindProperty]
        public Category Category { get; set; }
        public DeleteModel(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void OnGet(int id)
        {
            if (id != null && id != 0)
            {
                Category = context.Categories.Find(id);
            }
        }

        public IActionResult OnPost()
        {
            Category cat = context.Categories.Find(Category.Id);
            if (cat == null)
            {
                return NotFound();
            }
            context.Remove(cat);
            context.SaveChanges();
            TempData["success"] = "Category has been deleted successfully";
            return RedirectToPage("Index");

        }
    }
}
