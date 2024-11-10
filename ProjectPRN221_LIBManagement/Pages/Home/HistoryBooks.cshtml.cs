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

        public int? UserId { get; set; }
        public void OnGet()
        {
            UserId = HttpContext.Session.GetInt32("UserID");
            updateStatus();
            statuses = PRN221_LibContext.Ins.Statuses.ToList();

            var query = PRN221_LibContext.Ins.Transactions
            .Include(x => x.Book)
            .Include(x => x.StatusNavigation)
            .Where(x => x.UserId == UserId);
            

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

        private void updateStatus()
        {
            PRN221_LibContext.Ins.Database.ExecuteSqlRaw(
           @"UPDATE Transactions
          SET Status = 3
          WHERE DueDate < GETDATE() AND ReturnDate IS NULL AND Status = 1;");
        }
    }
}
