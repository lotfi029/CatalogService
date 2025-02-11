namespace Application.Features.Products.Contract;
public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl,
    int Quentity,
    float Price,
    Guid CategoryId
);
