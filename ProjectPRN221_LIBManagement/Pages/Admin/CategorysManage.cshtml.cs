using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class CategorysManageModel : PageModel
    {
        private readonly PRN221_LibContext _context;

        public CategorysManageModel(PRN221_LibContext context)
        {
            _context = context;
        }

        public IList<Category> Categorys { get; set; }

        public string search { get; set; }
        public async Task OnGetAsync(string searchString)
        {

            IQueryable<Category> categorysQuery = _context.Categories;


            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                categorysQuery = categorysQuery.Where(a => a.CategoryName.ToUpper().Contains(searchString));
                search = searchString;
            }

            Categorys = await categorysQuery.ToListAsync();


        }
    }
}
