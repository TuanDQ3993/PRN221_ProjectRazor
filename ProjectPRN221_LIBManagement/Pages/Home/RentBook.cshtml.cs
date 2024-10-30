using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Home
{
    public class RentBookModel : PageModel
    {
        private readonly PRN221_LibContext _context;

        public RentBookModel(PRN221_LibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int BookId { get; set; }

        [BindProperty]
        public DateTime ReturnDate { get; set; }

        [BindProperty]
        public DateTime BorrowDate { get; set; }

        public Book? Book { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                Redirect("/Home/ShopDetails");
            }

            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            Book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (Book == null)
            {
                Redirect("/Home/ShopDetails");
            }

            BookId = Book.BookId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            // Load book information again
            Book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.BookId == BookId);

            if (Book == null || Book.Quantity <= 0)
            {
                ErrorMessage = "Book is not available for rent.";
                return Page();
            }

            // Check active rentals
            var activeRentals = await _context.Transactions
                .CountAsync(t => t.UserId == userId && t.Status == 4);
            if (activeRentals >= 3)
            {
                ErrorMessage = "You can only rent up to 3 books at a time.";
                return Page();
            }

            // Validate borrow date
            if (BorrowDate < DateTime.Today)
            {
                ErrorMessage = "Borrow date cannot be in the past.";
                return Page();
            }

            var maxBorrowDate = DateTime.Today.AddDays(7);
            if (BorrowDate > maxBorrowDate)
            {
                ErrorMessage = "Borrow date cannot be more than 7 days from today.";
                return Page();
            }

            if (ReturnDate <= BorrowDate)
            {
                ErrorMessage = "Return date must be after borrow date.";
                return Page();
            }

            var maxReturnDate = BorrowDate.AddDays(30);
            if (ReturnDate > maxReturnDate)
            {
                ErrorMessage = "Return date cannot be more than 30 days from borrow date.";
                return Page();
            }

            var transaction = new Transaction
            {
                UserId = userId.Value,
                BookId = BookId,
                BorrowDate = BorrowDate,
                DueDate = ReturnDate,
                Status = 4 // Dki muon
            };

            Book.Quantity--;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            // Redirect to book details page with success message
            TempData["SuccessMessage"] = "Book rented successfully!";
            return RedirectToPage("/Home/ShopDetails", new { id = BookId });
        }
    }
}