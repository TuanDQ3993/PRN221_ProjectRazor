using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages
{
    public class ChangePasswordModel : PageModel
    {
        [BindProperty]
        public string OldPassword { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            var user = PRN221_LibContext.Ins.Users.SingleOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return Page();
            }

            
            if (user.Password != OldPassword)
            {
                ModelState.AddModelError("OldPassword", "Mật khẩu cũ không chính xác.");
                return Page();
            }

            
            if (NewPassword != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Mật khẩu xác nhận không khớp.");
                return Page();
            }

            
            user.Password = NewPassword;
            PRN221_LibContext.Ins.Users.Update(user);
            PRN221_LibContext.Ins.SaveChanges();

            TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công!";
            return RedirectToPage("ChangePassword"); 
        }
    }
}
