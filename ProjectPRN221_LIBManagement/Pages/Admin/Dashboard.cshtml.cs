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
        public async Task OnGetAsync()
        {
            transactions = await _context.Transactions
                .GroupBy(t => t.BorrowDate.Month)
                .Select(x => new Transaction
                {
                    Month = x.Key,
                    BorrowCount = x.Count(t => t.Status == 1),
                    ReturnCount = x.Count(t => t.Status == 2)
                }).ToListAsync();
        }

    }
}
