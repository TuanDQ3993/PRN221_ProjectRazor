using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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

        public PaginatedList<Book> Books { get; set; }

        [BindProperty]
        public IFormFile ExcelFile { get; set; }
        public IList<Author> authors = new List<Author>();
        public IList<Publisher> publishers = new List<Publisher>();
        public IList<Category> categories = new List<Category>();

        public int? pId { get; set; }
        public int? aId {get; set;}

        public int? cid { get; set; }
        
        public string search {  get; set; }
        public async Task OnGetAsync(string searchString, int? publisherId, int? authorId, int? categoryId, int? pageIndex)
        {
            int pageSize = 10;
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
            Books = await PaginatedList<Book>.CreateAsync(booksQuery, pageIndex ?? 1, pageSize);
           

        }
        public async Task<IActionResult> OnPostImportAsync()
        {
            if (ExcelFile == null || ExcelFile.Length == 0)
            {
                ModelState.AddModelError("ExcelFile", "Please select a valid Excel file.");
                return Page();
            }

            var books = new List<Book>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await ExcelFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var book = new Book
                        {
                            Title = worksheet.Cells[row, 1].Text,
                            AuthorId = int.Parse(worksheet.Cells[row, 2].Text),
                            PublisherId = int.Parse(worksheet.Cells[row, 3].Text),
                            YearPublished = int.Parse(worksheet.Cells[row, 4].Text),
                            Description = worksheet.Cells[row, 5].Text,
                            Isbn = worksheet.Cells[row, 6].Text,
                            CategoryId = int.Parse(worksheet.Cells[row, 7].Text),
                            Quantity = int.Parse(worksheet.Cells[row,8].Text),
                            Image = worksheet.Cells[row, 9].Text,

                        };
                        books.Add(book);
                    }
                }
            }

            _context.Books.AddRange(books);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Books imported successfully!";
            return RedirectToPage();
        }
    }
}
