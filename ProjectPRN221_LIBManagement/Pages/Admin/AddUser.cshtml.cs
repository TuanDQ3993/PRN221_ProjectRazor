using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class AddUserModel : PageModel
    {
        [BindProperty]
        public int UserId { get; set; }
        [BindProperty]
        public string FullName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public int Roleid { get; set; }
        [BindProperty]
        public string phoneNumber { get; set; }
        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public DateTime DateOfBirth { get; set; }

        private readonly PRN221_LibContext _context;

        public AddUserModel(PRN221_LibContext context)
        {
            _context = context;
        }

        public List<Role> roles { get; set; }

        public void OnGet()
        {
            roles = _context.Roles.ToList();
        }
        public IActionResult OnPost()
        {
            if (_context.Users.Any(u => u.Email == Email))
            {
                ModelState.AddModelError("Email", "Email exist already!");
                roles = _context.Roles.ToList();
                return Page();
            }

            User users = new User()
            {
                FullName = FullName,
                Email = Email,
                Password = HashPassword(Password),
                Role = Roleid,
                Address = Address,
                PhoneNumber = phoneNumber,
                DateOfBirth = DateOfBirth,
                IsBan = false
            };
            _context.Users.Add(users);
            _context.SaveChanges();
            return Redirect("UserManage");
        }

        public string HashPassword(string password)
        {

            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
