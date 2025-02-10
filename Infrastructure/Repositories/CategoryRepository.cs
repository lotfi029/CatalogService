namespace Infrastructure.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<Guid>> AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        if (category is null)
            return Result.Failure<Guid>(CategoryErrors.InvalidOperation);

        await _context.Categories.AddAsync(category, cancellationToken);

        return Result.Success(category.Id);
    }

    public async Task<Result> UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        if (category is null)
            return CategoryErrors.NullException;

        if (!await _context.Categories.AnyAsync(e => e.Id == category.Id, cancellationToken))
            return CategoryErrors.NotFound;

        _context.Categories.Update(category);

        return Result.Success();
    }

    public async Task<Result<Category>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (await _context.Categories.FindAsync([id], cancellationToken) is not { } category)
            return Result.Failure<Category>(CategoryErrors.NotFound);

        return Result.Success(category);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _context.Categories.ToListAsync(cancellationToken);
        return categories;
    }
}
