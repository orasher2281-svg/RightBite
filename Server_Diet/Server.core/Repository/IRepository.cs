using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IRepository<T>
    {
        Task<T?> GetById(int id);
        Task<int> Add(T entity);
        Task<T?> Update(T entity);
        Task<int> DeleteById(int id);

    }
}
