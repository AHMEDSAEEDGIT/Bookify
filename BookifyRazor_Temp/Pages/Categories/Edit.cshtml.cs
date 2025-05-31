using BookifyRazor_Temp.Data;
using BookifyRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookifyRazor_Temp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext context;
        [BindProperty]
        public Category Category { get; set; }
        public EditModel(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void OnGet(int id)
        {
            if(id != null && id != 0)
            {
                Category = context.Categories.Find(id);
            }

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                context.Update(Category);
                context.SaveChanges();
                TempData["success"] = "Category has been updated successfully";
                return RedirectToPage("Index");
            }

            return Page();

        }
    }
}
