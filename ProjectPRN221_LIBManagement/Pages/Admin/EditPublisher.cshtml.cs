using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class EditPublisherModel : PageModel
    {
        private readonly PRN221_LibContext _context;
        public EditPublisherModel(PRN221_LibContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Publisher publishers { get; set; } = new Publisher();
        public void OnGet(string? Id)
        {
            int? publisherID = int.Parse(Id);
            publishers = _context.Publishers.Find(publisherID);
        }
        public IActionResult OnPost()
        {
            var publisher = _context.Publishers.Find(publishers.PublisherId);
            publisher.PublisherName = publishers.PublisherName;
            _context.Publishers.Update(publisher);
            _context.SaveChanges();
            return RedirectToPage("PublisherManage");
        }
    }
}
