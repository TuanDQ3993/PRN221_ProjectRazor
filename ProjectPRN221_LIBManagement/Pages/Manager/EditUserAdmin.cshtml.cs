using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin2
{
    public class EditUserAdminModel : PageModel
    {
        private readonly PRN221_LibContext _context;
        public EditUserAdminModel(PRN221_LibContext context)
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
        public IActionResult OnPost()
        {
            var user = _context.Users.Find(users.UserId);
            user.FullName = users.FullName;
            user.Email = users.Email;
            user.Role = users.Role;
            user.PhoneNumber = users.PhoneNumber;
            user.Address = users.Address;
            user.DateOfBirth = users.DateOfBirth;
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToPage("UserManage");
        }
    }
}
