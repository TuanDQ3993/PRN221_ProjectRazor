using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using ProjectPRN221_LIBManagement.Models;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class EditBookModel : PageModel
    {
        private readonly PRN221_LibContext _context;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public Book books { get; set; } = new Book();

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public List<Author> authors = new List<Author>();
        public List<Publisher> publishers = new List<Publisher>();
        public List<Category> categories = new List<Category>();

        public EditBookModel(PRN221_LibContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public void OnGet(string id)
        {
            int bookId = int.Parse(id);
            books = _context.Books.Find(bookId);
            authors = _context.Authors.ToList();
            publishers = _context.Publishers.ToList();
            categories = _context.Categories.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var book = _context.Books.Find(books.BookId);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = books.Title;
            book.AuthorId = books.AuthorId;
            book.Description = books.Description;
            book.Quantity = books.Quantity;
            book.CategoryId = books.CategoryId;
            book.PublisherId = books.PublisherId;
            book.YearPublished = books.YearPublished;
            book.Isbn = books.Isbn;

            // Nếu có ảnh mới, lưu ảnh và cập nhật URL của ảnh trong database
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string imageUrl = await SaveImageAsync();
                if (imageUrl == null)
                {
                    ModelState.AddModelError("ImageFile", "Error saving image");
                    return Page();
                }
                book.Image = imageUrl;
            }

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToPage("BooksManage");
        }

        private async Task<string> SaveImageAsync()
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("ImageFile", "Allowed formats: .jpg, .jpeg, .png, .gif");
                return null;
            }

            var filePath = Path.Combine(_environment.WebRootPath, "images", Guid.NewGuid() + fileExtension);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));  // Ensure directory exists

            try
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                await ImageFile.CopyToAsync(stream);
                return "/images/" + Path.GetFileName(filePath);
            }
            catch (Exception)
            {
                ModelState.AddModelError("ImageFile", "Error saving image");
                return null;
            }
        }

    }
}
