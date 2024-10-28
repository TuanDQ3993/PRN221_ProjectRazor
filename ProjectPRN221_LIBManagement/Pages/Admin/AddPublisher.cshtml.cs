using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class AddPublisherModel : PageModel
    {
        [BindProperty]
        public int PublisherId { get; set; }
        [BindProperty]
        public string publisherName { get; set; }
        
        private readonly PRN221_LibContext _context;

        public AddPublisherModel(PRN221_LibContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            Publisher publisher = new Publisher()
            {
                PublisherName = publisherName
            };
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
            return Redirect("PublisherManage");
        }
    }
}
