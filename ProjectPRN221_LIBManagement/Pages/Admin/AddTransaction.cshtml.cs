using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;
using System.Net;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class AddTransactionModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string ISBN { get; set; }

        [BindProperty]
        public DateTime ReturnDate { get; set; }

        [BindProperty]
        public DateTime BorrowDate { get; set; }

        [BindProperty]
        public int Condition { get; set; }

        public List<BookCondition> bookCondition { get; set; }
        public void OnGet()
        {
            bookCondition=PRN221_LibContext.Ins.BookConditions.ToList();
        }

        public IActionResult OnPost() {
            
            if (!PRN221_LibContext.Ins.Users.Any(u => u.Email == Email))
            {
                ModelState.AddModelError("Email", "Email don't exist in the system!");
                return Page();
            }
            if (!PRN221_LibContext.Ins.Books.Any(b => b.Isbn == ISBN))
            {
                ModelState.AddModelError("ISBN", "Book don't exist in the system!");
                return Page();
            }

            User user= PRN221_LibContext.Ins.Users.FirstOrDefault(u=>u.Email == Email);
            Book book=PRN221_LibContext.Ins.Books.FirstOrDefault(b =>b.Isbn == ISBN);
            if(user!=null && book!=null)
            {
                var transaction = new Transaction
                {
                    UserId = user.UserId,
                    BookId = book.BookId,
                    BorrowDate = BorrowDate,
                    DueDate = ReturnDate,
                    Status = 1, // Dki muon
                    BookConditionOnBorrow = Condition
                };
                PRN221_LibContext.Ins.Transactions.Add(transaction);
                PRN221_LibContext.Ins.SaveChanges();
            }

            return Redirect("TransactionsManage");

        }
    }
}
