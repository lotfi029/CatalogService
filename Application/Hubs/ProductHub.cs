using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs;
public class ProductHub : Hub<IProductClient>
{
}
