using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class DeleteBookModel : PageModel
    {
        private readonly PRN221_LibContext _context;
        public DeleteBookModel(PRN221_LibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Book books { get; set; } = new Book();
        public List<Author> authors = new List<Author>();
        public List<Publisher> publishers = new List<Publisher>();
        public List<Category> categories = new List<Category>();
        public void OnGet(string id)
        {
            int bookId = int.Parse(id);
            books = _context.Books.Find(bookId);
            authors = _context.Authors.ToList();
            publishers = _context.Publishers.ToList();
            categories = _context.Categories.ToList();
        }

        public IActionResult OnPost(int id) 
        {
            var book = _context.Books.Find(id); 
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges(); 
                return RedirectToPage("BooksManage");
            }
            return Page(); 
        }

    }
}

