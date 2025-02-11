using Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class FileSerivce(
    IWebHostEnvironment webHostEnvironment,
    ApplicationDbContext _context) : IFileService
{
    private readonly string _videoPath = @$"{webHostEnvironment.WebRootPath}\files\lessons\videos";
    private readonly string _imagePath = @$"{webHostEnvironment.WebRootPath}\images";

    //public async Task<Guid> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
    //{
    //    var uploadFile = await SaveFileToServer(file, cancellationToken);

    //    await _context.UploadedFiles.AddAsync(uploadFile, cancellationToken);
    //    await _context.SaveChangesAsync(cancellationToken);

    //    return uploadFile.Id;
    //}
    public async Task<string> UploadImageAsync(IFormFile image, CancellationToken token = default)
    {
        //string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

        var path = Path.Combine(_imagePath, image.FileName);

        using var stream = File.Create(path);

        await image.CopyToAsync(stream, token);

        return path;
    }
    //public async Task<IEnumerable<Guid>> UploadManyAsync(IFormFileCollection files, CancellationToken cancellationToken = default)
    //{
    //    List<UploadedFile> uploadFiles = [];

    //    foreach (var file in files) 
    //        uploadFiles.Add(await SaveFileToServer(file, cancellationToken));

    //    await _context.UploadedFiles.AddRangeAsync(uploadFiles, cancellationToken);
    //    await _context.SaveChangesAsync(cancellationToken);

    //    var response = uploadFiles.Select(e => e.Id);

    //    return response;
    //}
    //public async Task<(byte[] fileContent, string contentType, string fileName)> DownloadAsync(Guid id, CancellationToken cancellationToken = default)
    //{
    //    var file = await _context.UploadedFiles.FindAsync([id], cancellationToken);

    //    if (file is null)
    //        return ([], string.Empty, string.Empty);

    //    var path = Path.Combine(_videoPath, file.StoredFileName);

    //    MemoryStream memoryStream = new();
    //    using FileStream fileStream = new(path, FileMode.Open);
    //    fileStream.CopyTo(memoryStream);

    //    memoryStream.Position = 0;

    //    return (memoryStream.ToArray(), file.ContentType, file.FileName);

    //}
    //public async Task<(FileStream? stream, string contentType, string fileName)> StreamAsync(Guid id, CancellationToken cancellationToken = default)
    //{
    //    var file = await _context.UploadedFiles.FindAsync([id], cancellationToken);

    //    if (file is null)
    //        return (null,  string.Empty, string.Empty);

    //    var path = Path.Combine(_videoPath, file.StoredFileName);

    //    var fileStream = File.OpenRead(path);

    //    return (fileStream, file.ContentType, file.FileName);
    //}
    //private async Task<UploadedFile> SaveFileToServer(IFormFile file, CancellationToken cancellationToken = default)
    //{
    //    var randomFileName = Path.GetRandomFileName();

    //    var uploadFile = new UploadedFile
    //    {
    //        FileName = file.FileName,
    //        ContentType = file.ContentType,
    //        StoredFileName = randomFileName,
    //        FileExtension = Path.GetExtension(file.FileName)
    //    };

    //    var path = Path.Combine(_videoPath, randomFileName);
    //    using var stream = File.Create(path);
    //    await file.CopyToAsync(stream, cancellationToken);

    //    return uploadFile;
    //}
}