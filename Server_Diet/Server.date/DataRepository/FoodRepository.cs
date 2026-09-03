using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Repository;
using Microsoft.EntityFrameworkCore;
using Server.date;

namespace Data.DataRepository
{
    public class FoodRepository : IFoodRepository
    {
        private readonly DietContext _dietContext;
        public FoodRepository(DietContext dietContext)
        {
            _dietContext = dietContext;
        }
        public async Task<int> Add(Food entity)
        {
            _dietContext.Foods.Add(entity);
             await _dietContext.SaveChangesAsync();
             return entity.Id; // מחזיר את ה-ID שהתעדכן באובייקט
        }
        public async Task<Food[]> AddFoods(Food[] entity)
        {
            foreach (Food entityItem in entity)
            {
                _dietContext.Foods.Add(entityItem);
            }
            await _dietContext.SaveChangesAsync();
            return entity;
        }
        public async Task<int> DeleteById(int id)
        {
            Food f = await _dietContext.Foods.FindAsync(id);
            _dietContext.Foods.Remove(f);
            return await _dietContext.SaveChangesAsync();
        }

        public async Task<Food?> GetById(int id)
        {
            return await _dietContext.Foods.FirstOrDefaultAsync(x=>x.Id==id);
        }
        public async Task<List<Food>> GetAll()
        {
            return await _dietContext.Foods.ToListAsync();
        }
        public async  Task<Food?> Update(Food entity)
        {
            Food f = await _dietContext.Foods.FindAsync(entity.Id);
            _dietContext.Entry(f).CurrentValues.SetValues(entity);
             await _dietContext.SaveChangesAsync();
            return f;
        }
        public async Task<IEnumerable<Food>> searchFood(string nameFood)
        {
            if (string.IsNullOrEmpty(nameFood))
            {
                return Enumerable.Empty<Food>();
            }
            string cleanSearch= nameFood.ToLower().Trim();
            string[] searchWords=cleanSearch.Split(' ',StringSplitOptions.RemoveEmptyEntries);

            if (searchWords.Length == 0) {
                return Enumerable.Empty<Food>();
            }
            string firstWord = searchWords[0];
            List<Food> foods = await _dietContext.Foods.Where(f => searchWords.Any(word => f.Name.ToLower().Contains(word))).OrderByDescending(f => f.Name.ToLower().StartsWith(firstWord)).ToListAsync();  
            return foods;
        
        }
            
    }
}
