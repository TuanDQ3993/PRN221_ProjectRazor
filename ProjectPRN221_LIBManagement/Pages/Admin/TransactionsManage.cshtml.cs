using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement.Pages.Admin
{
    public class TransactionsManageModel : PageModel
    {
        public List<Status> status { get; set; }
        public List<Transaction> transactions { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Status { get; set; } = 0;

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SearchType { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetInt32("UserRole");

            if (role == null || role != 1) 
            {
                return RedirectToPage("/Home/AccessDenied"); 
            }
            status = PRN221_LibContext.Ins.Statuses.ToList();
            transactions = FilterTransactions(); // Lấy dữ liệu theo điều kiện lọc
            return Page();
        }

        private List<Transaction> FilterTransactions()
        {
            var query = PRN221_LibContext.Ins.Transactions
                .Include(x => x.Book)
                .Include(x => x.StatusNavigation)
                .Include(x => x.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Search))
            {
                query = SearchType == 0
                    ? query.Where(x => x.User.FullName.Contains(Search))
                    : query.Where(x => x.Book.Title.Contains(Search));
            }

            if (Status > 0)
            {
                query = query.Where(x => x.Status == Status);
            }

            if (StartDate.HasValue)
            {
                query = query.Where(x => x.BorrowDate >= StartDate.Value);
            }

            if (EndDate.HasValue)
            {
                query = query.Where(x => x.BorrowDate <= EndDate.Value);
            }

            return query.ToList();
        }

        public IActionResult OnPost()
        {
            // Lọc lại dữ liệu theo điều kiện trước khi xuất
            var filteredTransactions = FilterTransactions();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Transactions");

                // Thêm tiêu đề cột
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "User Name";
                worksheet.Cell(1, 3).Value = "Book Title";
                worksheet.Cell(1, 4).Value = "Borrow Date";
                worksheet.Cell(1, 5).Value = "Due Date";
                worksheet.Cell(1, 6).Value = "Return Date";
                worksheet.Cell(1, 7).Value = "Status";

                // Điền dữ liệu vào bảng
                for (int i = 0; i < filteredTransactions.Count; i++)
                {
                    var transaction = filteredTransactions[i];
                    worksheet.Cell(i + 2, 1).Value = transaction.TransactionId;
                    worksheet.Cell(i + 2, 2).Value = $"{transaction.User.FullName} ({transaction.User.Email})";
                    worksheet.Cell(i + 2, 3).Value = transaction.Book.Title;
                    worksheet.Cell(i + 2, 4).Value = transaction.BorrowDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(i + 2, 5).Value = transaction.BorrowDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(i + 2, 6).Value = transaction.ReturnDate?.ToString("yyyy-MM-dd") ?? "Chưa trả";
                    worksheet.Cell(i + 2, 7).Value = transaction.StatusNavigation.StatusName;
                }

                // Định dạng bảng
                worksheet.Columns().AdjustToContents();

                // Trả về file Excel dưới dạng file tải về
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Transactions.xlsx");
                }
            }
        }
    }
}
