using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class ProductHub : Hub<IProductClient>
{
    public async Task ServerMethonName(string user, string product)
    {
        await Clients.All.ProductAdded($"{user} add the {product}");
    }
    public async Task ProductUpdated(string product)
    {
        await Clients.All.ProductUpdated(product);
    }
    public async Task ProductDeleted(int productId)
    {
        await Clients.All.ProductDeleted(productId);
    }
}

public class NotificationHub : Hub<INotificationClient>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Client(Context.ConnectionId).ReceiveNotification("Welcome to the notification hub");

        await base.OnConnectedAsync();
    }
}

public interface INotificationClient
{
    Task ReceiveNotification(string message);
}
public interface IProductClient
{
    Task ProductAdded(string product);
    Task ProductUpdated(string product);
    Task ProductDeleted(int productId);
}