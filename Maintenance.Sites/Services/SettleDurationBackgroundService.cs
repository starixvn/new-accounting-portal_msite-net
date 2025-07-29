namespace Maintenance.Sites.Services;

public class SettleDurationBackgroundService(
    ILogger<SettleDurationBackgroundService> logger,
    SettleDurationService service) : BackgroundService
{
    private readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // UTC+7

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("SettleDurationBackgroundService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, _timeZone);
                var nextRunTime = GetNextRunTime(now);

                var delay = nextRunTime - now;
                if (delay.TotalMilliseconds > 0)
                {
                    await Task.Delay(delay, stoppingToken);
                }

                await service.SendLetstalk();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred in SettleDurationBackgroundService");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        logger.LogInformation("SettleDurationBackgroundService is stopping.");
    }

    private DateTime GetNextRunTime(DateTime currentTime)
    {
        var nextRunTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 14, 0, 0); // 2 PM

        // If it's already past 2 PM today, schedule for tomorrow
        if (currentTime.TimeOfDay >= TimeSpan.FromHours(14))
        {
            nextRunTime = nextRunTime.AddDays(1);
        }

        return nextRunTime;
    }
}