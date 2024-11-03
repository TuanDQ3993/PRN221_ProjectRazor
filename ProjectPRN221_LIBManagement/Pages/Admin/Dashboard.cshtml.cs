using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly PRN221_LibContext _context;
        public DashboardModel(PRN221_LibContext context)
        {
            _context = context;
        }

        public IList<Transaction> transactions { get; set; }
        public IList<Transaction> transactionsCount { get; set; }

         public List<Book>? BookHot { get; set; }
        public int TotalUsers { get; set; }
        public int TotalBooks { get; set; } 
        public int TotalCategory {  get; set; }

        public int TotalTransaction {  get; set; }
        public async Task OnGetAsync()
        {
            TotalUsers = await _context.Users.CountAsync();

            TotalBooks = await _context.Books.CountAsync();

            TotalCategory = await _context.Categories.CountAsync();

            TotalTransaction = await _context.Transactions.CountAsync(); 

            transactions = await _context.Transactions
                .GroupBy(t => t.BorrowDate.Month)
                .Select(x => new Transaction
                {
                    Month = x.Key,
                    BorrowCount = x.Count(t => t.Status == 1),
                    ReturnCount = x.Count(t => t.Status == 2)
                }).ToListAsync();

            transactionsCount = await _context.Transactions.GroupBy(x => x.Status).Select(g => new Transaction
            {
                Status = g.Key,
                StatusCount = g.Count()
            }).ToListAsync();

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
