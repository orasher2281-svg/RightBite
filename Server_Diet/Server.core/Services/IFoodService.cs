using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Repository;

namespace Core.Services
{
    public interface IFoodService:IService<Food>
    {
        Task<List<Food>> GetAll();
        Task<IEnumerable<Food>> searchFood(string nameFood);
        Task<Food[]> AddFoods(Food[] entity);
       
    }
}
