using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Models;
using Core.Repository;
using Microsoft.EntityFrameworkCore;
using Server.date;

namespace Data.DataRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DietContext _dietContext;
        public UserRepository(DietContext dietContext)
        {
            _dietContext = dietContext;
        }
        public async Task<int> Add(User entity)

        {
            _dietContext.Users.Add(entity);
            return await _dietContext.SaveChangesAsync();
            
        }

        public async Task<int> DeleteById(int id)
        {
            User u = await _dietContext.Users.FindAsync(id);
            _dietContext.Users.Remove(u);
            return await _dietContext.SaveChangesAsync();   
        }

        public async  Task<User?> GetByEmail(string email)
        {
           var u=await _dietContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return u;
        }

        public async Task<User?> GetById(int id)
        {
            return await  _dietContext.Users.FirstOrDefaultAsync(x=>x.Id==id);
        }
        public async Task<List<User>> GetAll()
        {
           return await _dietContext.Users.ToListAsync();
        }
        public async Task<User?> Update(User entity)
        {
            User? u=await _dietContext.Users.FindAsync(entity.Id);
            if (u == null)
            {
                return null; 
            }
            _dietContext.Entry(u).CurrentValues.SetValues(entity);
            await  _dietContext.SaveChangesAsync();
            return u;
        }

       
    }
}
