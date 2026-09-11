using Microsoft.EntityFrameworkCore;
using RolexWatches.Application.Interfaces.Repositories;
using RolexWatches.Domain.Entities;
using RolexWatches.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolexWatches.Infrastructure.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> NameExistsAsync(string name, int? excludeCategoryId = null)
        {
            return await DbSet.AnyAsync(c =>
                c.Name == name && (excludeCategoryId == null || c.Id != excludeCategoryId));
        }

        public async Task<List<Category>> GetTopLevelWithChildrenAsync()
        {
            return await DbSet
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync();
        }

        public async Task<bool> HasProductsAsync(int categoryId)
        {
            return await Context.Set<Product>().AnyAsync(p => p.CategoryId == categoryId);
        }
    }
}