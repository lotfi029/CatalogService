namespace Application.Hubs;
public interface IProductClient
{
    Task ProductAdded(ProductResponse product);
    Task ProductUpdated(ProductResponse product);
    Task ProductDeleted(int productId);
}
