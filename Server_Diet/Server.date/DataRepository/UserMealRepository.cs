using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Repository;
using Microsoft.EntityFrameworkCore;
using Server.date;

namespace Data.DataRepository
{
    public class UserMealRepository : IUserMealRepository
    {
        private readonly DietContext _dietContext;
        public UserMealRepository(DietContext dietContext)
        {
            _dietContext = dietContext;
        }
        public async Task<int> Add(UserMeal entity)
        {
            _dietContext.UserMeals.Add(entity);
            return await _dietContext.SaveChangesAsync();
        }

        public async Task<int> DeleteById(int id)
        {
            int us = await _dietContext.UserMeals.Where(x => x.Id == id).ExecuteDeleteAsync();
            return us;
        }

        public async Task<UserMeal?> GetById(int id)
        {
            return await _dietContext.UserMeals.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserMeal?> Update(UserMeal entity)
        {
            UserMeal um = await _dietContext.UserMeals.FindAsync(entity.Id);
            if (um == null) {
                return null;
            }
            _dietContext.Entry(um).CurrentValues.SetValues(entity);
            await  _dietContext.SaveChangesAsync();
            return um;
        }
        public IQueryable<UserMeal> GetUserMealsQuery()
        {
            return _dietContext.UserMeals;
        }
       
    }
}
