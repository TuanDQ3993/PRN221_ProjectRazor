using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class TransactionDetailModel : PageModel
    {
        public Transaction transaction { get; set; }

        public void OnGet(string tid)
        {
            try
            {
                int id = int.Parse(tid);
                transaction = PRN221_LibContext.Ins.Transactions
                .Include(t => t.BookConditionOnReturnNavigation).Include(t=>t.BookConditionOnBorrowNavigation)
                .Include(t => t.StatusNavigation)
                .FirstOrDefault(t => t.TransactionId == id);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
