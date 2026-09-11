using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RolexWatches.Application.Interfaces.Repositories;
using RolexWatches.Domain.Entities;
using RolexWatches.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace RolexWatches.Infrastructure.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbcontext;
        public CategoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> NameExistsAsync(string name, int? excludeCategoryId = null)
        {
            this.dbcontext = dbcontext;
            return await DbSet.AnyAsync(c =>
                c.Name == name && (excludeCategoryId == null || c.Id != excludeCategoryId));
        }

        public async Task AddAsync(Category category)
        public async Task<List<Category>> GetTopLevelWithChildrenAsync()
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
            return await DbSet
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync();
        }

        public void Update(Category category)
        public async Task<bool> HasProductsAsync(int categoryId)
        {
            dbcontext.Categories.Update(category);
            return await Context.Set<Product>().AnyAsync(p => p.CategoryId == categoryId);
        }
    }
}
