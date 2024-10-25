using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class EditCategoryModel : PageModel
    {
        [BindProperty]
        public Category Category { get; set; } = new Category();
        public void OnGet(String id)
        {
            int categoryId = int.Parse(id);
            Category = PRN221_LibContext.Ins.Categories.Find(categoryId);
        }

        public IActionResult OnPost()
        {
            var category = PRN221_LibContext.Ins.Categories.Find(Category.CategoryId);
            if (category != null)
            {
                category.CategoryName = Category.CategoryName;
                PRN221_LibContext.Ins.Categories.Update(category);
                PRN221_LibContext.Ins.SaveChanges();
            }

            return RedirectToPage("CategorysManage"); ;

        }
    }
}
