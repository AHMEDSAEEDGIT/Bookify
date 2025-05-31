using BookifyRazor_Temp.Data;
using BookifyRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookifyRazor_Temp.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext context;

        [BindProperty] // should use to let the model binder bind the object from request to your property 
        public Category Category { get; set; }
        public CreateModel(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPost() {
            context.Add(Category);
            context.SaveChanges();
            TempData["success"] = "Category has been created successfully";
            return RedirectToPage("Index");
        }
    }
}
