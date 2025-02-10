using Application.Features.Categories.Contracts;
using Application.Features.Categories.Queries;

namespace Application.Features.Categories.Handlers;

public sealed class GetCategoryByIdQueryHandler(ICategoryRepository _categoryRepository)
    : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponse>>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(query.Id, cancellationToken);

        if (category.IsFailure)
            return Result.Failure<CategoryResponse>(category.Error);

        var response = category.Value.Adapt<CategoryResponse>();

        return Result.Success(response);
    }
}
