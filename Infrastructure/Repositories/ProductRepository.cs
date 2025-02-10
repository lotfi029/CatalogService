using Application.Features.Products.Contract;
using Application.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Repositories;
public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<Guid>> AddProductAsync(Product request, CancellationToken cancellationToken = default)
    {
        if (request is null)
            return Result.Failure<Guid>(ProductErrors.InvalidOperation);

        await _context.Products.AddAsync(request, cancellationToken);

        return Result.Success(request.Id);
    }
    public async Task<Result> UpdateProductAsync(Product request, CancellationToken cancellationToken = default)
    {
        if (request is null)
            return ProductErrors.NullException;

        if (!await _context.Products.AnyAsync(e => e.Id == request.Id, cancellationToken))
            return ProductErrors.NotFound;

        _context.Products.Update(request);
        
        return Result.Success();
    }
    public async Task<Result<Product>> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (await _context.Products.FindAsync([id], cancellationToken) is not { } product)
            return Result.Failure<Product>(ProductErrors.NotFound);

        return Result.Success(product);
    }

    public async Task<IEnumerable<Product>> GetAllProductAsync(bool? includeDisabled = null, CancellationToken cancellationToken = default)
    {
        var products = await _context.Products
            .Where(e => includeDisabled == null || includeDisabled == e.IsDisabled)
            .ToListAsync(cancellationToken);

        return products;
    }


}
