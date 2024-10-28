using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class DeletePublisherModel : PageModel
    {
        private readonly PRN221_LibContext _context;
        public DeletePublisherModel(PRN221_LibContext context)
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
        public IActionResult OnPost(int id)
        {
            var publisher = _context.Publishers.Find(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                _context.SaveChanges();
                return RedirectToPage("PublisherManage");
            }
            return Page();
        }
    }
}
