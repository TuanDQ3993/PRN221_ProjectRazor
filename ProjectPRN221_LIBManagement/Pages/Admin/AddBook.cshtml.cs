using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using ProjectPRN221_LIBManagement.Models;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public string Description { get; set; }
        [BindProperty]
        public string Isbn { get; set; }
        [BindProperty]
        public int CategoryId { get; set; }
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public IFormFile ImageFile { get; set; }

        private readonly IWebHostEnvironment _environment;
        private readonly PRN221_LibContext _context;

        public List<Author> authors { get; set; } = new List<Author>();
        public List<Publisher> publishers { get; set; } = new List<Publisher>();
        public List<Category> categories { get; set; } = new List<Category>();

        public AddBookModel(PRN221_LibContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            // Tải danh sách tác giả, nhà xuất bản và thể loại
            authors = _context.Authors.ToList();
            publishers = _context.Publishers.ToList();
            categories = _context.Categories.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string imageUrl = await SaveImageAsync();
            if (imageUrl == null)
            {
                ModelState.AddModelError("ImageFile", "Error saving image");
                return Page();
            }

            var book = new Book
            {
                Title = Title.Trim(),
                AuthorId = AuthorId,
                Description = Description.Trim(),
                PublisherId = PublisherId,
                YearPublished = YearPublished,
                Isbn = Isbn?.Trim(),
                CategoryId = CategoryId,
                Quantity = Quantity,
                Image = imageUrl
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Book added successfully!";

            return RedirectToPage("BooksManage");
        }

        private async Task<string> SaveImageAsync()
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                var fileExtension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ImageFile", "Only .jpg, .jpeg, .png and .gif files are allowed");
                    return null;
                }

                var uniqueFileName = Guid.NewGuid() + fileExtension;

                var imagesPath = Path.Combine(_environment.WebRootPath, "images");
                Directory.CreateDirectory(imagesPath);

                var filePath = Path.Combine(imagesPath, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                return "/images/" + uniqueFileName;
            }

            return null;
        }
    }
}
