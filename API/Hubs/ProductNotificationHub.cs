using Application.Features.Products.Contract;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class ProductNotificationHub : Hub<IProductClientd>
{
    public async Task ServerMethonName(string user, string product)
    {
        //await Clients.All.ProductAdded($"{user} add the {product}");
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
public interface IProductClientd
{
    Task ProductAdded(ProductResponse product);
    Task ProductUpdated(string product);
    Task ProductDeleted(int productId);
}

public class ProductNotification(IHubContext<ProductNotificationHub, IProductClientd> _hubContext) : IProductClientd
{
    public async Task ProductAdded(ProductResponse product)
    {
        await _hubContext.Clients.All.ProductAdded(product);
    }

    public Task ProductDeleted(int productId)
    {
        throw new NotImplementedException();
    }

    public Task ProductUpdated(string product)
    {
        throw new NotImplementedException();
    }
}