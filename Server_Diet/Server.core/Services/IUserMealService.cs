using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Resource;

namespace Core.Services
{
    public interface IUserMealService:IService<UserMeal>
    {
         Task<DailyNutritionSummaryResource?> GetDailyNutritionSummaryAsync(int userID, DateTime date);
        Task<List<UserMealResource>> GetUserMealsByDateAsync(int userID, DateTime date);
    }
}
