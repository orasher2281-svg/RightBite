using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Services
{
    public interface IService<T>
    {
        Task<T?> GetById(int id);
        Task<int> Add(T entity);
        Task<T?> Update(T entity);
        Task<int> DeleteById(int id);
    }
}
