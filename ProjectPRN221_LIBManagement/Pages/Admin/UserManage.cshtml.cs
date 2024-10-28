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
        public int? statusFilter { get; set; } // 0: Active, 1: Banned, null: All

        public async Task OnGetAsync(string searchString, int? roleId, int? statusFilter)
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

            // Filter by status (BIT)
            if (statusFilter.HasValue)
            {
                bool isBanned = statusFilter.Value == 1;
                usersQuery = usersQuery.Where(u => u.IsBan == isBanned);
                this.statusFilter = statusFilter;
            }

            users = await usersQuery.ToListAsync();
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(int userId)
        {
            try
            {
                var user = await PRN221_LibContext.Users.FindAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                // Toggle BIT value
                user.IsBan = !user.IsBan;
                await PRN221_LibContext.SaveChangesAsync();

                // Redirect back to the same page with existing filters
                return RedirectToPage(new
                {
                    searchString = search,
                    roleId = roleId,
                    statusFilter = statusFilter
                });
            }
            catch
            {
                TempData["ErrorMessage"] = "Failed to update user status.";
                return RedirectToPage();
            }
        }

        // Helper method để lấy text hiển thị status
        public string GetStatusText(bool isBan)
        {
            return isBan ? "Banned" : "Active";
        }

        public string GetStatusClass(bool isBan)
        {
            return isBan ? "btn-danger" : "btn-success";
        }
    }
}
