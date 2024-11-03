using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class ChangeStatusModel : PageModel
    {
        public IActionResult OnGet(int transId, int value)
        {
            var transaction=PRN221_LibContext.Ins.Transactions.Find(transId);
            transaction.Status = value;
            if (transaction.Status == 2) { 
            transaction.ReturnDate = DateTime.Now;
            }
            PRN221_LibContext.Ins.Transactions.Update(transaction);
            PRN221_LibContext.Ins.SaveChanges();

            return RedirectToPage("TransactionsManage");
        }
    }
}
