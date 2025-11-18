using Microsoft.EntityFrameworkCore.Storage;

namespace CatalogService.Domain.IRepositories;
public interface IUnitOfWork 
{
    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }
    IProductVariantRepository ProductVariants { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync (IDbContextTransaction transaction, CancellationToken ct = default);
    Task RollBackTransactionAsync(IDbContextTransaction transaction, CancellationToken ct = default);
}
