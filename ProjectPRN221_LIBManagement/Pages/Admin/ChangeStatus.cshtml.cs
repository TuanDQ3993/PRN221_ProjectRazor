using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class ChangeStatusModel : PageModel
    {
        public IActionResult OnGet(int transId, int value,int bookCondition)
        {
            Console.WriteLine(bookCondition);
            var transaction=PRN221_LibContext.Ins.Transactions.Find(transId);
            transaction.Status = value;
            if (transaction.Status == 2) { 
            transaction.BookConditionOnReturn= bookCondition;
            transaction.ReturnDate = DateTime.Now;
            }
            if(transaction.Status == 1)
            {
                transaction.BookConditionOnBorrow = bookCondition;
            }
            PRN221_LibContext.Ins.Transactions.Update(transaction);
            PRN221_LibContext.Ins.SaveChanges();

            return RedirectToPage("TransactionsManage");
        }
    }
}
