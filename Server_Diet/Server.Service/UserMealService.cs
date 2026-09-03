using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core;
using Core.Models;
using Core.Repository;
using Core.Resource;
using Core.Services;
using Microsoft.EntityFrameworkCore;
namespace Service
{
    public class UserMealService : IUserMealService
    {
        private readonly IUserMealRepository _userMealREpository;
        private readonly IFoodAnalysisService _aiService;
        public UserMealService(IUserMealRepository userMealRepository, IFoodAnalysisService aiService   )
        {
            _userMealREpository = userMealRepository;
            _aiService = aiService;
        }
        public async Task<int> Add(UserMeal entity)
        {
            return await _userMealREpository.Add(entity);
        }

        public async Task<int> DeleteById(int id)
        {
            return await _userMealREpository.DeleteById(id);
        }

        public async Task<UserMeal?> GetById(int id)
        {
            return await _userMealREpository.GetById(id);
        }

        public async Task<UserMeal> Update(UserMeal entity)
        {
            return await _userMealREpository.Update(entity);
        }
        public async Task<DailyNutritionSummaryResource?> GetDailyNutritionSummaryAsync(int userID, DateTime date)
        {
            DailyNutritionSummaryResource DailyNutrition =await  _userMealREpository.GetUserMealsQuery().Where(f => f.UserId == userID && f.MealDate.Date == date.Date)
                .GroupBy(g => new
                {
                    g.UserId,
                    MealDate = g.MealDate.Date, 

                    // נתינת שמות מפורשים בתוך ה-GroupBy כדי למנוע בלבול:
                    CaloriesTarget = g.User.DailyCalories,
                    CarbsTarget = g.User.TargetCarbs,
                    ProteinTarget = g.User.TargetProtein,
                    FatTarget = g.User.TargetFat
                })
                .Select(u => new DailyNutritionSummaryResource
                {
                    UserId = userID,
                    Date = date,
                    DailyCalories = u.Key.CaloriesTarget,
                    TargetCarbs = u.Key.CarbsTarget,
                    TargetProtein = u.Key.ProteinTarget,
                    TargetFat = u.Key.FatTarget,
                    CurrentCalories = u.Sum(x => (x.Quantity / 100.0) * x.Food.Calories),
                    CurrentCarbs = u.Sum(x => (x.Quantity / 100.0) * x.Food.Carbs),
                    CurrentProtein = u.Sum(x => (x.Quantity / 100.0) * x.Food.Protein),
                    CurrentFat = u.Sum(x => (x.Quantity / 100.0) * x.Food.Fat)
                }).FirstOrDefaultAsync();
            return DailyNutrition;
        }
        public async Task<List<UserMealResource>> GetUserMealsByDateAsync(int userID, DateTime date)
        {
            List<UserMealResource> userMealResourceList =await _userMealREpository.GetUserMealsQuery().Where(u=>u.MealDate.Date==date.Date && u.UserId==userID)
                .Select(x=> new UserMealResource {
                    Id = x.Id,
                    UserId =x.UserId,
                    FoodId= x.FoodId,
                    Quantity= x.Quantity,
                    MealType= x.MealType,
                    MealDate= x.MealDate,
                    Food=x.Food !=null? new FoodResource
                    {
                        Id = x.Food.Id,
                        Name=x.Food.Name,
                        Description=x.Food.Description,
                        ImageUrl= x.Food.ImageUrl,
                        Calories = (x.Quantity / 100.0) * x.Food.Calories,
                        Carbs = (x.Quantity / 100.0) * x.Food.Carbs,
                        Protein = (x.Quantity / 100.0) * x.Food.Protein,
                        Fat = (x.Quantity / 100.0) * x.Food.Fat

                    } :null

                }).ToListAsync();
                return userMealResourceList;
        }




       

    }
}
