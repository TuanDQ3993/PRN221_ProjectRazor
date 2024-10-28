using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages
{
    public class profileModel : PageModel
    {

        [BindProperty]
        public User user { get; set; }
        public void OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId != null)
            {
                user = PRN221_LibContext.Ins.Users.SingleOrDefault(u => u.UserId == userId);
            }
        }

        public IActionResult OnPost() {
            int? userId = HttpContext.Session.GetInt32("UserID");

            // Nếu không có userId trong session, redirect đến trang đăng nhập
            if (userId == null)
            {
                return RedirectToPage("login");
            }

            var userDb = PRN221_LibContext.Ins.Users.SingleOrDefault(u => u.UserId == userId);
            
            if (userDb != null)
            {

                if (user.DateOfBirth >= DateTime.Today)
                {
                    ModelState.AddModelError("user.DateOfBirth", "Ngày sinh phải nhỏ hơn ngày hiện tại.");
                    return Page(); // Trả lại trang với lỗi
                }
                userDb.FullName = user.FullName;
                userDb.PhoneNumber = user.PhoneNumber;
                userDb.Address = user.Address;
                userDb.DateOfBirth = user.DateOfBirth;

                PRN221_LibContext.Ins.Users.Update(userDb);
                PRN221_LibContext.Ins.SaveChanges(); // Lưu thay đổi vào database
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return Page();
            }
            ModelState.AddModelError(string.Empty, "Không tìm thấy thông tin người dùng.");
            return RedirectToPage("login");
        }
    }
}
