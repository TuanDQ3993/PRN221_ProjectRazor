using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class AddAuthorModel : PageModel
    {
        [BindProperty]
        public Author Author { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
          

            PRN221_LibContext.Ins.Authors.Add(Author);
            PRN221_LibContext.Ins.SaveChanges();
            return Redirect("AuthorsManage");
        }
    }
}
