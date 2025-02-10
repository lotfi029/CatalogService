namespace Application.Features.Products.Contract;

public record UpdateProductRequest(
    string Name,
    string Description,
    int Quentity,
    float Price
);