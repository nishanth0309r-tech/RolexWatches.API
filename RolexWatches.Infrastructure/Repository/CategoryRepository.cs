using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Infrastructure.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly ApplicationDbContext dbcontext;

        public CategoryRepository(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task AddAsync(Category category)
        {
             await dbcontext.Categories.AddAsync(category);
        }

        public void Delete(Category category)
        {
            dbcontext.Categories.Remove(category);  
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await dbcontext.Categories.ToListAsync(); 
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
           return await dbcontext.Categories.FindAsync(id); 
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await dbcontext.SaveChangesAsync() > 0;
        }

        public void Update(Category category)
        {
            dbcontext.Categories.Update(category);
        }
    }
}
