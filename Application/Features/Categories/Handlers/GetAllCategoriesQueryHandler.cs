using Application.Features.Categories.Contracts;
using Application.Features.Categories.Queries;

namespace Application.Features.Categories.Handlers;

public sealed class GetAllCategoriesQueryHandler(ICategoryRepository _categoryRepository)
    : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryResponse>>
{
    public async Task<IEnumerable<CategoryResponse>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        
        var response = categories.Adapt<IEnumerable<CategoryResponse>>();

        return response;
    }
}