using Core.Models;
using Core.Services;
using Server.date;

namespace Web_Api.Service
{
    public class FileService : IFileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DietContext _db;

        public FileService(IHttpContextAccessor httpContextAccessor, DietContext db)
        {
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }

        public async Task<ProcessedFileResult> SaveFileAsync(Stream fileStream, string originalFileName)
        {
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();

            var ext = Path.GetExtension(originalFileName).ToLowerInvariant();
            var contentType = ext switch
            {
                ".png" => "image/png",
                ".webp" => "image/webp",
                ".gif" => "image/gif",
                _ => "image/jpeg"
            };

            var image = new FoodImage { Data = imageBytes, ContentType = contentType };
            _db.FoodImages.Add(image);
            await _db.SaveChangesAsync();

            string imageUrl = string.Empty;
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request != null)
            {
                imageUrl = $"{request.Scheme}://{request.Host}/api/Image/{image.Id}";
            }

            return new ProcessedFileResult { ImageUrl = imageUrl, ImageBytes = imageBytes };
        }
    }
}