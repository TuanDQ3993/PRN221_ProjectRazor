using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Home
{
    public class ShopDetailsModel : PageModel
    {
        private readonly PRN221_LibContext _context;
        public Book? BookDetails { get; set; }
        public List<Book>? RelatedBook { get; set; }
        public List<Category>? Categories { get; set; }

        public int? UserId { get; set; }
        public ShopDetailsModel(PRN221_LibContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            UserId = HttpContext.Session.GetInt32("UserID");
            //bookDetail
            BookDetails = await _context.Books
                        .Include(b => b.Author)
                        .Include(b => b.Category)
                        .Include(b => b.Publisher)
                        .FirstOrDefaultAsync(b => b.BookId == id);

            //related book
            RelatedBook = await _context.Books
                        .Include(b => b.Author)
                        .Include(b => b.Category)
                        .Include(b => b.Publisher)
                        .Where(b => b.CategoryId == BookDetails.CategoryId && b.BookId != id && b.Quantity >0)
                        .Take(3)
                        .ToListAsync();

            Categories = await _context.Categories.Include(c => c.Books).ToListAsync();
            return Page();

        }
    }
}
