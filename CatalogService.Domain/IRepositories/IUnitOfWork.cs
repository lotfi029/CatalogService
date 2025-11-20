using Microsoft.EntityFrameworkCore.Storage;

namespace CatalogService.Domain.IRepositories;
public interface IUnitOfWork 
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync (IDbContextTransaction transaction, CancellationToken ct = default);
    Task RollBackTransactionAsync(IDbContextTransaction transaction, CancellationToken ct = default);
}
