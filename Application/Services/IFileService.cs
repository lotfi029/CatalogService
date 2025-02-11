using Microsoft.AspNetCore.Http;

namespace Application.Services;
public interface IFileService
{
    Task<string> UploadImageAsync(IFormFile image, CancellationToken token = default);
}

