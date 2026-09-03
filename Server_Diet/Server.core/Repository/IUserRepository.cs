using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Repository
{
    public interface IUserRepository:IRepository<User>
    {
        Task<User?> GetByEmail(string email);
        Task<List<User>> GetAll();
    }
}
