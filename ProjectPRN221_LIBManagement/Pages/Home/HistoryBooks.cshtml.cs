using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProjectPRN221_LIBManagement.Pages.Home
{
    public class HistoryBooksModel : PageModel
    {

        public List<Transaction> Transactions { get; set; }

        public List<Status> statuses { get; set; }

        [BindProperty(SupportsGet = true)] 
        public string Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Status { get; set; } = 0;
        public void OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            statuses = PRN221_LibContext.Ins.Statuses.ToList();

            var query = PRN221_LibContext.Ins.Transactions
            .Include(x => x.Book)
            .Include(x => x.StatusNavigation)
            .Where(x => x.UserId == userId);
            

            if (!string.IsNullOrEmpty(Search))
            {
                query = query.Where(x => x.Book.Title.Contains(Search));
            }

            if (Status > 0)
            {
                query = query.Where(x => x.Status == Status);
            }

            Transactions = query.ToList();

        }
    }
}
