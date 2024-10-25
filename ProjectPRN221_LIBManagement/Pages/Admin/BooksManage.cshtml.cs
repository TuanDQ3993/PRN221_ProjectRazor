using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class BooksManageModel : PageModel
    {
        private readonly PRN221_LibContext _context;

        public BooksManageModel(PRN221_LibContext context)
        {
            _context = context;
        }
        public IList<Book> Books { get; set; }
        public IList<Author> authors = new List<Author>();
        public IList<Publisher> publishers = new List<Publisher>();
        public IList<Category> categories = new List<Category>();

        public int? pId { get; set; }
        public int? aId {get; set;}

        public int? cid { get; set; }
        
        public string search {  get; set; }
        public async Task OnGetAsync(string searchString, int? publisherId, int? authorId, int? categoryId)
        {

            IQueryable<Book> booksQuery = _context.Books.Include(x => x.Author)
                                                        .Include(x => x.Category)
                                                        .Include(x => x.Publisher);
            authors = _context.Authors.ToList();
            publishers = _context.Publishers.ToList();
            categories = _context.Categories.ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                booksQuery = booksQuery.Where(b => b.Title.ToUpper().Contains(searchString) ||
                                                   b.Isbn.ToUpper().Contains(searchString));
                search = searchString;
            }
            if (authorId> 0)
            {
                booksQuery = booksQuery.Where(b => b.AuthorId == authorId);
              aId = authorId;
            }
            if (categoryId > 0) {
                booksQuery = booksQuery.Where(b => b.CategoryId == categoryId);
                cid= categoryId;
            }
            if (publisherId > 0) {
                booksQuery = booksQuery.Where(b => b.PublisherId == publisherId);
                pId = publisherId;
            }




            Books = await booksQuery.ToListAsync();
           

        }
    }
}
