using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class AddBookModel : PageModel
    {
        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public int AuthorId { get; set; }

        [BindProperty]
        public int? PublisherId { get; set; }

        [BindProperty]
        public int YearPublished { get; set; }
        [BindProperty]
        public string Isbn { get; set; }
        [BindProperty]
        public int CategoryId { get; set; }
        [BindProperty]
        public int Quantity { get; set; }

        private readonly PRN221_LibContext _context;
        public AddBookModel(PRN221_LibContext context)
        {
            _context = context;
        }

        public List<Author> authors = new List<Author>();
        public List<Publisher> publishers = new List<Publisher>();
        public List<Category> categories = new List<Category>();
      
            
        public void OnGet()
        {
            authors = _context.Authors.ToList();
            publishers = _context.Publishers.ToList();
            categories = _context.Categories.ToList();
        }
        public IActionResult OnPost() {
            Book books = new Book()
            {
                Title = Title,
                AuthorId = AuthorId,
                PublisherId = PublisherId,
                YearPublished = YearPublished,
                Isbn = Isbn,
                CategoryId = CategoryId,
                Quantity = Quantity,
            };
            _context.Books.Add(books);
            _context.SaveChanges();
            return Redirect("BooksManage");
        }
    }
}
