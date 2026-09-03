using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Repository;
using Core.Services;

namespace Service
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        public FoodService(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }
        public async Task<int> Add(Food entity)
        {
            return await _foodRepository.Add(entity);
        }
        
        public async Task<Food[]> AddFoods(Food[] entity)
        {
            return await _foodRepository.AddFoods(entity);
        }
        public async Task<int> DeleteById(int id)
        {
            return await _foodRepository.DeleteById(id);
        }

        public async Task<List<Food>> GetAll()
        {
            return await _foodRepository.GetAll();
        }

        public async Task<Food?> GetById(int id)
        {
            return await _foodRepository.GetById(id);
        }

        public async Task<Food?> Update(Food entity)
        {
           return await (_foodRepository.Update(entity));           
        }
        public async Task<IEnumerable<Food>> searchFood(string nameFood)
        {
            return await _foodRepository.searchFood(nameFood);
        }

    }
}
