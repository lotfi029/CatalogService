namespace Application.Features.Products.Contract;
public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    int Quentity,
    float Price,
    Guid CategoryId
);
