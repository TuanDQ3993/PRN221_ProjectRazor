using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("123");
            Console.WriteLine(hashedPassword);
            var user = PRN221_LibContext.Ins.Users
                                   .SingleOrDefault(u => u.Email == Username);
            if (user != null && VerifyPassword(Password, user.Password))
            {
                if (user.IsBan == true)
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản đã bị khóa.");
                }
                else
                {

                    HttpContext.Session.SetInt32("UserID", user.UserId);
                    HttpContext.Session.SetInt32("UserRole", user.Role);
                    if (user.Role == 3)
                    {
                        return RedirectToPage("/Admin/BooksManage");
                    }
                    else if(user.Role == 3)
                    {
                        return RedirectToPage("/Admin2/librianManage");
                    }
                    else
                    {

                        return RedirectToPage("home/home");
                    }

                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            return Page();

        }

        public bool VerifyPassword(string enteredPassword, string hashedPassword)
        {

            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }
    }
}
