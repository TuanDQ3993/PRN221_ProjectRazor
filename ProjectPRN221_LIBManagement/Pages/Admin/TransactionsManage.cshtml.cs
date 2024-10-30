using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class TransactionsManageModel : PageModel
    {
        public List<Status> status { get;set;}

        public List<Transaction> transactions { get;set;}


        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }


        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }


        [BindProperty(SupportsGet = true)]
        public int Status { get; set; } = 0;


        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SearchType { get; set; }


        public void OnGet()
        {
            status= PRN221_LibContext.Ins.Statuses.ToList();

            var query = PRN221_LibContext.Ins.Transactions
            .Include(x => x.Book)
            .Include(x => x.StatusNavigation).Include(x=>x.User).AsQueryable();


            if (!string.IsNullOrEmpty(Search))
            {
                if (SearchType == 0) {
                    query = query.Where(x => x.User.FullName.Contains(Search));
                }
                else
                {
                    query = query.Where(x => x.Book.Title.Contains(Search));
                }
               
            }

            if (Status > 0)
            {
                query = query.Where(x => x.Status == Status);
            }

            if (StartDate.HasValue)
            {
                query = query.Where(x => x.BorrowDate >= StartDate.Value);
            }

            if (EndDate.HasValue)
            {
                query = query.Where(x => x.BorrowDate <= EndDate.Value);
            }


            transactions = query.ToList();
        }
    }
}
