using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.date;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly DietContext _db;
        public ImageController(DietContext db) { _db = db; }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var img = await _db.FoodImages.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            if (img == null) return NotFound();
            Response.Headers.CacheControl = "public, max-age=31536000";
            return File(img.Data, img.ContentType);
        }
    }
}
