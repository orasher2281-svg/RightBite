using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Resource;

namespace Core.Services
{
    public interface IUserService:IService<User>
    {
         Task<int> Register(User u);
        Task<int> Login(LoginResource loginResource);
        void CalculateUserNutritionGoals(User user);
        Task<List<User>> GetAll();
    }
}
