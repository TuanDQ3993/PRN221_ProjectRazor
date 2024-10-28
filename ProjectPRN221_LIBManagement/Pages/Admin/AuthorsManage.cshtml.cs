using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class AuthorsManageModel : PageModel
    {
        private readonly PRN221_LibContext _context;

        public AuthorsManageModel(PRN221_LibContext context)
        {
            _context = context;
        }
       
        public IList<Author> Authors { get; set; }

        public string search { get; set; }
        public async Task OnGetAsync(string searchString)
        {

            IQueryable<Author> authorsQuery = _context.Authors;
            

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                authorsQuery = authorsQuery.Where(a => a.AuthorName.ToUpper().Contains(searchString));
                search = searchString;
            }

            Authors = await authorsQuery.ToListAsync();


        }
    }
}
