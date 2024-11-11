using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectPRN221_LIBManagement.Pages.Admin2
{
    public class AdminDashboard1Model : PageModel
    {
        private readonly PRN221_LibContext _context;
        public AdminDashboard1Model(PRN221_LibContext context)
        {
            _context = context;
        }

        public IList<Transaction> transactions { get; set; }
        public IList<Transaction> transactionsCount { get; set; }

        public List<Book>? BookHot { get; set; }
        public int TotalUsers { get; set; }
        public int TotalBooks { get; set; }
        public int TotalCategory { get; set; }

        public int TotalTransaction { get; set; }


        public async Task OnGetAsync()
        {
            TotalUsers = await _context.Users.CountAsync();

            TotalBooks = await _context.Books.CountAsync();

            TotalCategory = await _context.Categories.CountAsync();

            TotalTransaction = await _context.Transactions.CountAsync();

        }
    }
}
