using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class BooksManageModel : PageModel
    {
        private readonly PRN221_LibContext _context;

        public BooksManageModel(PRN221_LibContext context)
        {
            _context = context;
        }
        public IList<Book> Books { get; set; }

        public async Task OngetAsync()
        {
            Books = await _context.Books.Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.Publisher).ToListAsync();
        }
    }
}
