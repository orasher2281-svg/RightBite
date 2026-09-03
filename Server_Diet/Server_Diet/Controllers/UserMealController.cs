using AutoMapper;
using Core.Models;
using Core.Resource;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMealController : ControllerBase
    {
        private readonly IUserMealService _userMealService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IFoodAnalysisService _aiService;
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;

        public UserMealController(IUserMealService userMealService, IMapper mapper, ILogger<UserMealController> logger, IFoodAnalysisService aiService, IWebHostEnvironment env  , IFileService fileService )
        {
            _userMealService = userMealService;
            _mapper = mapper;
            _logger = logger;
            _aiService = aiService;
            _env = env;
            _fileService= fileService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserMealResource?>> GetById(int id)
        {
            var userMeal = await _userMealService.GetById(id);
            return Ok(_mapper.Map<UserMealResource>(userMeal));
        }
        [Authorize] //  אם אין טוקן תקין, הבקשה תיעצר כאן ב-401.
        [HttpPost]
        public async Task<ActionResult<int>> Add([FromBody] UserMealResource u)
        {
            if (u.MealDate == null)
            {
                u.MealDate = DateTime.Now;
            }
            UserMeal userMeal = _mapper.Map<UserMeal>(u);
            
            return Ok(await _userMealService.Add(userMeal));
        }
        [Authorize] // אם אין טוקן תקין, הבקשה תיעצר כאן ב-401.
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteById(int id)
        {
            return Ok(await _userMealService.DeleteById(id));
        }
        [Authorize] //  אם אין טוקן תקין, הבקשה תיעצר כאן ב-401.
        [HttpPut]
        public async Task<ActionResult<UserMealResource?>> Update(UserMealResource u)
        {
            var updatedUserMeal = _mapper.Map<UserMeal>(u);
            if (updatedUserMeal == null)
            {
                return NotFound($"User with ID {u.Id} not found.");
            }
            var newUserMeal = await _userMealService.Update(updatedUserMeal);
            return Ok(_mapper.Map<UserMealResource>(newUserMeal));

        }
        [HttpGet("GetDailyNutrition")]
        public async Task<ActionResult<DailyNutritionSummaryResource?>> GetDailyNutritionSummaryAsync([FromQuery] int id, [FromQuery] DateTime date)
        {

            return Ok(await _userMealService.GetDailyNutritionSummaryAsync(id, date));
        }
        [HttpGet("GetUserMealsByDate")]
        public async Task<ActionResult<List<UserMealResource>>> GetUserMealsByDateAsync([FromQuery] int id, [FromQuery] DateTime date)
        {
            return Ok(await _userMealService.GetUserMealsByDateAsync(id, date));
        }
        [HttpPost("analyze")]
        public async Task<ActionResult<NutritionalInfoResource>> AnalyzeMeal([FromForm] AnalyzeMealRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FoodName) && (request.File == null || request.File.Length == 0))
            {
                return BadRequest("חובה להזין לפחות שם של מאכל או להעלות תמונה.");
            }

            ProcessedFileResult? fileResult = null;

            // אם יש קובץ, פותחים Stream ושולחים לשירות השמירה
            if (request.File != null && request.File.Length > 0)
            {
                using var stream = request.File.OpenReadStream();
                fileResult = await _fileService.SaveFileAsync(stream, request.File.FileName);
            }

            byte[]? imageBytes = fileResult?.ImageBytes;

            // שליחה ל-AI לצורך ניתוח התזונה
            var nutritionData = await _aiService.AnalyzeAsync(request.FoodName, request.Description, imageBytes);

            // חיבור ה-URL שחזר מהשירות לתוצאה שתחזור ללקוח
            if (fileResult != null)
            {
                nutritionData.ImageUrl = fileResult.ImageUrl;
            }

            return Ok(nutritionData);
        }
    }
}
