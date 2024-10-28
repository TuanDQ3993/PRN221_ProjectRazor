using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;
using Microsoft.EntityFrameworkCore;
namespace ProjectPRN221_LIBManagement.Pages.Home
{
    public class HomeModel : PageModel
    {
        private readonly PRN221_LibContext _context;
        public List<Book>? BookNew { get; set; }
        public List<Book>? BookHot { get; set; }

        public HomeModel(PRN221_LibContext context)
        {
            _context = context;
        }

        public void OnGet()
        {

            //booknew
            BookNew = _context.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .Include(b => b.Publisher)
                    .Where(x => x.Quantity > 0)
                    .OrderByDescending(x => x.BookId)
                    .Take(8)
                    .ToList();

            //bookhot
            var transactionCounts = _context.Transactions
                .GroupBy(t => t.BookId)
                .Select(g => new { BookId = g.Key, Count = g.Count() })
                .ToList();

            var hotBooks = _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Where(b => b.Quantity > 0)
                .ToList();

            foreach (var book in hotBooks)
            {
                book.RentalCount = transactionCounts
                    .FirstOrDefault(t => t.BookId == book.BookId)?.Count ?? 0;
            }

            BookHot = hotBooks
                .OrderByDescending(b => b.RentalCount)
                .Take(6)
                .ToList();
        }

    }
}
