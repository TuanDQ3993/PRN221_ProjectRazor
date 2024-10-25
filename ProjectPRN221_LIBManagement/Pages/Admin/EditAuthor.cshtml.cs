using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class EditAuthorModel : PageModel
    {
        [BindProperty]
        public Author Author { get; set; }=new Author();
        public void OnGet(String id)
        {
            int authorId = int.Parse(id);
            Author=PRN221_LibContext.Ins.Authors.Find(authorId);
        }

        public IActionResult OnPost() { 
        var author = PRN221_LibContext.Ins.Authors.Find(Author.AuthorId);
            if (author != null) { 
            author.AuthorName = Author.AuthorName;
            PRN221_LibContext.Ins.Authors.Update(author);
            PRN221_LibContext.Ins.SaveChanges();
            }

            return RedirectToPage("AuthorsManage"); ;

        }
    }
}
