using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Repository
{
    public interface IFoodRepository:IRepository<Food>
    {
        Task<List<Food>> GetAll();
        Task<IEnumerable<Food>> searchFood(string nameFood);
        Task<Food[]> AddFoods(Food[] entity);
    }
}
