using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{   

    public class UserManageModel : PageModel
    {   
        private readonly PRN221_LibContext PRN221_LibContext;
        public UserManageModel(PRN221_LibContext PRN221_LIBContext)
        {
            PRN221_LibContext = PRN221_LIBContext;
        }
        public IList<User> users { get; set; }
        public IList<Role> roles { get; set; }
        public int? roleId { get; set; }
        public string search { get; set; }
        public async Task OnGetAsync(string searchString, int? roleId)
        {
            IQueryable<User> usersQuery = PRN221_LibContext.Users.Include(x => x.RoleNavigation);
            roles = await PRN221_LibContext.Roles.ToListAsync();

            // Filter by search string (FullName)
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                usersQuery = usersQuery.Where(u => u.FullName.ToUpper().Contains(searchString));
                search = searchString;
            }

            // Filter by role
            if (roleId > 0)
            {
                usersQuery = usersQuery.Where(u => u.Role == roleId);
                this.roleId = roleId;
            }
            users = await usersQuery.ToListAsync();
        }
    }
}
