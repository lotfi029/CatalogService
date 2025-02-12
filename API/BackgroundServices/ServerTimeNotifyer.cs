
using API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace API.BackgroundServices;

public class ServerTimeNotifyer(
    ILogger<ServerTimeNotifyer> _logger,
    IHubContext<NotificationHub, INotificationClient> _hubContext
    ) : BackgroundService
{
    private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Period);

        while (!stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            var currentTime = DateTime.Now;

            _logger.LogInformation("Executing : {server} {currentTime}", nameof(ServerTimeNotifyer), currentTime);

            await _hubContext.Clients.All.ReceiveNotification($"Server time: {currentTime}");
        }
    }
}
