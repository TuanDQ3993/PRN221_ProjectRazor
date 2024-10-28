using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class DeleteUserModel : PageModel
    {
        private readonly PRN221_LibContext _context;
        public DeleteUserModel(PRN221_LibContext context)
        {
            _context = context;
        }
        public IList<Role> roles { get; set; }
        [BindProperty]
        public User users { get; set; } = new User();
        public void OnGet(string? Id)
        {
            int? userID = int.Parse(Id);
            users = _context.Users.Find(userID);
            roles = _context.Roles.ToList();
        }
        public IActionResult OnPost(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return RedirectToPage("UserManage");
            }
            return Page();
        }
    }
}
