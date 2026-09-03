using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Repository
{
    public interface IUserMealRepository:IRepository<UserMeal>
    {
        public IQueryable<UserMeal> GetUserMealsQuery();
    }
}
