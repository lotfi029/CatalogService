namespace Infrastructure.Repositories;
public class UnitOfWork(
    ApplicationDbContext context
    ) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;

    public void Dispose() =>
        _context.Dispose();
    

    public Task<int> SaveChangeAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
    
}
