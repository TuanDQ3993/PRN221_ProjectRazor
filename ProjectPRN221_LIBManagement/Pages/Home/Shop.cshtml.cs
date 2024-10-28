using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;
using System.Linq;
using System.Security.Cryptography;

namespace ProjectPRN221_LIBManagement.Pages.Home
{
    public class ShopModel : PageModel
    {
        private readonly PRN221_LibContext _context;
        public ShopModel(PRN221_LibContext context)
        {
            _context = context;
        }

        public IList<Book> Books { get; set; }
        public IList<Category> Categories { get; set; } = new List<Category>();
        public IList<Author> Author { get; set; }
        public IList<Publisher> Publisher { get; set; }

        public int? selectedCate { get; set; }
        public int? selectedPub { get; set; }
        public int? selectedAu { get; set; }

        public string searched { get; set; }

        public string sortOrdered { get; set; }

        public int? UserId { get; set; }
        public async Task OnGetAsync(int? cateId, int? pubId, int? auId, string search, string sortOrder)
        {
            UserId = HttpContext.Session.GetInt32("UserID");

            IQueryable<Book> booksQuery = _context.Books.Include(x => x.Author)
                                                       .Include(x => x.Category)
                                                       .Include(x => x.Publisher);
            Categories = _context.Categories.ToList();
            Author = _context.Authors.ToList();
            Publisher = _context.Publishers.ToList();

            if (!string.IsNullOrEmpty(search))
            {
                searched = search;

                booksQuery = booksQuery.Where(b => b.Title.Contains(search) ||
                                                   b.Isbn.Contains(search));

            }

            if (cateId > 0)
            {
                booksQuery = booksQuery.Where(b => b.CategoryId == cateId);
                selectedCate = cateId;
            }

            if (pubId > 0)
            {
                booksQuery = booksQuery.Where(b => b.PublisherId == pubId);
                selectedPub = pubId;
            }

            if (auId > 0)
            {
                booksQuery = booksQuery.Where(b => b.AuthorId == auId);
                selectedAu = auId;
            }

            switch (sortOrder)
            {
                case "Ascending":
                    sortOrdered = "Ascending";
                    booksQuery = booksQuery.OrderBy(b => b.Title);
                    break;
                case "Descending":
                    sortOrdered = "Descending";
                    booksQuery = booksQuery.OrderByDescending(b => b.Title);
                    break;
                case "Default":
                    sortOrdered = "Default";
                    break;
                default:
                    break;
            }



            Books = await booksQuery.ToListAsync();

        }
    }
}
