using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class EditBookModel : PageModel
    {
        private readonly PRN221_LibContext _context;

        [BindProperty]
        public Book books { get; set; } = new Book();

        public List<Author> authors = new List<Author>();
        public List<Publisher> publishers = new List<Publisher>();
        public List<Category> categories = new List<Category>();

        public EditBookModel(PRN221_LibContext context)
        {
            _context = context;
        }

        public void OnGet(string id)
        {
            int bookId = int.Parse(id);
            books = _context.Books.Find(bookId);
            authors = _context.Authors.ToList();
            publishers = _context.Publishers.ToList();
            categories = _context.Categories.ToList();
        }

        public IActionResult OnPost()
        {
            var book = _context.Books.Find(books.BookId);

            if (book == null)
            {
                return NotFound();
            }

            book.Title = books.Title;
            book.AuthorId = books.AuthorId;  
            book.Quantity = books.Quantity;
            book.CategoryId = books.CategoryId;  
            book.PublisherId = books.PublisherId;  
            book.YearPublished = books.YearPublished;
            book.Isbn = books.Isbn;

            _context.Books.Update(book);
            _context.SaveChanges();

            return RedirectToPage("BooksManage");
        }


    }
}
