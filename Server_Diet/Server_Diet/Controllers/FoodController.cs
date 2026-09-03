using AutoMapper;
using Core.Models;
using Core.Resource;
using Core.Services;
using Data.DataRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : Controller
    {
        private readonly IFoodService _foodService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;


        public FoodController(IFoodService foodService, IMapper mapper, ILogger<FoodController> logger)
        {
            _foodService = foodService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodResource?>> GetById(int id)
        {
            var food = await _foodService.GetById(id);
            return Ok(_mapper.Map<FoodResource>(food));
        }
        [HttpGet("getAll")]
        public async Task<ActionResult<List<FoodResource>>> GetAll()
        {
            var foods = await _foodService.GetAll();
            return Ok(_mapper.Map<List<FoodResource>>(foods));
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Food>>> searchFood([FromQuery] string nameFood)
        {
            return Ok(await _foodService.searchFood(nameFood));
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> Add(FoodResource f)
        {
            var food = _mapper.Map<Food>(f);
            return Ok(await _foodService.Add(food));
        }
     
        [HttpPost("addFoods")]
        [Produces("application/json")]
        public async Task<ActionResult<FoodResource[]>> AddFoods(FoodResource[] f)
        {
            var food = _mapper.Map<Food[]>(f);
            return Ok(await _foodService.AddFoods(food));
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteById(int id)
        {
            return Ok(await _foodService.DeleteById(id));
        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult<FoodResource>> Update(FoodResource f)
        {
            var updatedFood = _mapper.Map<Food>(f);
            if (updatedFood == null)
            {
                return NotFound($"User with ID {f.Id} not found.");
            }
            var newFood = await _foodService.Update(updatedFood);
            return Ok(_mapper.Map<FoodResource>(newFood));

        }

    }
}
