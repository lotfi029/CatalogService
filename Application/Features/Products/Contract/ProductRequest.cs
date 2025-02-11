using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Contract;
public record ProductRequest(
    string Name,
    string Description,
    IFormFile Image,
    int Quentity,
    float Price,
    Guid CategoryId
);
