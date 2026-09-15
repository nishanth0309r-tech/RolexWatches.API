using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using RolexWatches.Domain.Enums;

namespace RolexWatches.Infrastructure.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<User>> GetAllCustomersAsync()
        {
            return await dbContext.Users.Where(u => u.Role == UserRole.Customer).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await dbContext.Users.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync() > 0;  
        }

        public void Update(User user)
        {
            dbContext.Users.Update(user);
        }
    }
}
