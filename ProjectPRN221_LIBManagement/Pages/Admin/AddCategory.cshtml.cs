using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class AddCategoryModel : PageModel
    {
        [BindProperty]
        public Category Category { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {


            PRN221_LibContext.Ins.Categories.Add(Category);
            PRN221_LibContext.Ins.SaveChanges();
            return Redirect("CategorysManage");
        }
    }
}
