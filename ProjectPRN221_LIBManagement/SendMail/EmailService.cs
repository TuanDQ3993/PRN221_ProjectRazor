using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using ProjectPRN221_LIBManagement.Models;

namespace ProjectPRN221_LIBManagement
{


    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendReminderEmails();
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly PRN221_LibContext _context;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IConfiguration configuration,
            PRN221_LibContext context,
            ILogger<EmailService> logger)
        {
            _emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
            _context = context;
            _logger = logger;
        }

        // Hàm gửi mail cơ bản
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, false);
            await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        // Hàm gửi mail nhắc nhở
        public async Task SendReminderEmails()
        {
            try
            {
                // Lấy danh sách transactions thỏa mãn điều kiện
                var transactions = _context.Transactions
                    .Include(t => t.Book)  
                    .Include(t => t.User)  
                    .Where(t => t.Status == 1
                        && t.ReturnDate == null
                        && t.DueDate == DateTime.Now.AddDays(1).Date)
                    .ToList();

                foreach (var transaction in transactions)
                {
                    // Kiểm tra null
                    if (transaction.User != null && transaction.Book != null)
                    {
                        string subject = "Lời nhắc: Sách của bạn sẽ đến hạn vào ngày mai!";
                        string body = $@"Xin chào {transaction.User.FullName},
                    <br><br>
                    Sách '{transaction.Book.Title}' của bạn sẽ đến hạn vào ngày mai({DateTime.Now.AddDays(1).ToString("dd/MM/yyyy")}). Hãy đảm bảo trả lại đúng hạn.
                    <br><br>
                    Thanks,<br>
                    JQKBook Library Team";

                        // Kiểm tra email không null và không rỗng
                        if (!string.IsNullOrEmpty(transaction.User.Email))
                        {
                            await SendEmailAsync(transaction.User.Email, subject, body);
                            _logger.LogInformation($"Sent reminder email to {transaction.User.Email}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SendReminderEmails: {ex.Message}");
                throw;
            }
        }



    }
}
