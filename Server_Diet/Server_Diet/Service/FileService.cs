using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Http;
namespace Web_Api.Service
{
  

    public class FileService : IFileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProcessedFileResult> SaveFileAsync(Stream fileStream, string originalFileName)
        {
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(originalFileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

            // בניית כתובת מלאה שכוללת את ה-Domain והשנל של ה-API (למשל https://localhost:7231/uploads/...)
            string imageUrl = string.Empty;
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request != null)
            {
                imageUrl = $"{request.Scheme}://{request.Host}/uploads/{uniqueFileName}";
            }

            return new ProcessedFileResult
            {
                ImageUrl = imageUrl,
                ImageBytes = imageBytes
            };
        }
    }
}
