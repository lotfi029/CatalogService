namespace Application.Features.Categories.Contracts;
public record CategoryResponse(
    Guid Id,
    string Name,
    string Description,
    bool IsDisabled
    );