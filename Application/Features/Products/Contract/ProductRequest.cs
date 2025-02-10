namespace Application.Features.Products.Contract;
public record ProductRequest(
    string Name,
    string Description,
    int Quentity,
    float Price,
    Guid CategoryId
);
