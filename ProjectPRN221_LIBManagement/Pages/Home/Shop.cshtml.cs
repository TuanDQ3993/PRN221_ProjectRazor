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

        public PaginatedList<Book> Books { get; set; }
  /*      public IList<Book> Books { get; set; }*/
        public IList<Category> Categories { get; set; } = new List<Category>();
        public IList<Author> Author { get; set; }
        public IList<Publisher> Publisher { get; set; }

        public int? selectedCate { get; set; }
        public int? selectedPub { get; set; }
        public int? selectedAu { get; set; }

        public string searched { get; set; }

        public string sortOrdered { get; set; }
        public async Task OnGetAsync(int? cateId, int? pubId, int? auId, string search, string sortOrder, int? pageIndex)
        {
            int pageSize = 9; // Số lượng sách trên mỗi trang
            IQueryable<Book> booksQuery = _context.Books.Include(x => x.Author)
                                                         .Include(x => x.Category)
                                                         .Include(x => x.Publisher);
            Categories = await _context.Categories.ToListAsync();
            Author = await _context.Authors.ToListAsync();
            Publisher = await _context.Publishers.ToListAsync();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                searched = search;
                booksQuery = booksQuery.Where(b => b.Title.Contains(search) || b.Isbn.Contains(search));
            }

            // Lọc theo thể loại, nhà xuất bản, tác giả
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

            // Sắp xếp
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

            // Tạo PaginatedList
            Books = await PaginatedList<Book>.CreateAsync(booksQuery, pageIndex ?? 1, pageSize);
        }
    }
}
