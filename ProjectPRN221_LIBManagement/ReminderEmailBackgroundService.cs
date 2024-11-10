using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProjectPRN221_LIBManagement.SendMail;
using ProjectPRN221_LIBManagement;

public class ReminderEmailBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ReminderEmailBackgroundService> _logger;

    public ReminderEmailBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<ReminderEmailBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Email reminder service running at: {time}", DateTimeOffset.Now);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    await emailService.SendReminderEmails();
                }

                // Chờ 24 giờ trước khi chạy lại
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending reminder emails");
                // Chờ 5 phút trước khi thử lại nếu có lỗi
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}