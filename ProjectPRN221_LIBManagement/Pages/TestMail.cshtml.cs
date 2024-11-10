using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProjectPRN221_LIBManagement.Pages
{
    public class TestMailModel : PageModel
    {
        private readonly IEmailService _emailService;

        public TestMailModel(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _emailService.SendEmailAsync(
                    "mihtuan199xxx@gmail.com",  // Email người nhận 
                    "Test Thử",
                    "Đây là email test"
                );
                TempData["Message"] = "Gửi mail thành công!";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Lỗi: {ex.Message}";
            }
            return RedirectToPage();
        }
    }
}
