using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class PublisherManageModel : PageModel
    {
        private readonly PRN221_LibContext PRN221_LibContext;
        public PublisherManageModel(PRN221_LibContext PRN221_LIBContext)
        {
            PRN221_LibContext = PRN221_LIBContext;
        }
        public IList<Publisher> publishers { get; set; }
        public string search { get; set; }
        public async Task OnGetAsync(string searchString, int? roleId)
        {
            IQueryable<Publisher> publishersQuery = PRN221_LibContext.Publishers;

            // Filter by search string (PublisherName)
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                publishersQuery = publishersQuery.Where(u => u.PublisherName.ToUpper().Contains(searchString));
                search = searchString;
            }
            publishers = await publishersQuery.ToListAsync();
        }
    }
}
