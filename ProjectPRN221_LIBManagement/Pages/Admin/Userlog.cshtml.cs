using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;
namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class UserlogModel : PageModel
    {
        private readonly PRN221_LibContext _context;
        public UserlogModel(PRN221_LibContext context)
        {
            _context = context;
        }
        public PaginatedList<UserBookLog> Logs { get; set; }
        public string search { get; set; }

        public List<BookCondition> conditions { get; set; }

        public int? conId { get; set; }
        
        public async Task OnGetAsync(int? pageIndex, string searchString, int? conditionID)
        { 
            int pageSize = 10;
            IQueryable<UserBookLog> logs = _context.UserBookLogs
           .Include(u => u.User)
           .Include(t => t.LastTransaction)
           .Include(b => b.LastBookConditionOnReturnNavigation);

            conditions = _context.BookConditions.ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                search = searchString;
                searchString = searchString.ToUpper();
                logs = logs.Where(u => u.Email.Contains(searchString) || u.FullName.Contains(searchString));

            }

            if (conditionID > 0) {
                conId = conditionID;
                logs = logs.Where(c => c.LastBookConditionOnReturn == conditionID);
            
            }


            Logs = await PaginatedList<UserBookLog>.CreateAsync(logs, pageIndex ?? 1, pageSize);
        }

    }
}