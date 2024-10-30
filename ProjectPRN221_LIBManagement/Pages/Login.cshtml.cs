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
			var user = PRN221_LibContext.Ins.Users
								   .SingleOrDefault(u => u.Email == Username && u.Password == Password);

			if (user != null)
			{
				if (user.Role != null)
				{
                    HttpContext.Session.SetInt32("UserID", user.UserId);
                    if (user.Role == 1)
					{
						return RedirectToPage("/Admin/BooksManage"); 
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
    }
}
