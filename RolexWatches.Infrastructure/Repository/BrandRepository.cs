using Microsoft.EntityFrameworkCore;
using RolexWatches.Application.Interfaces.Repositories;
using RolexWatches.Domain.Entities;
using RolexWatches.Infrastructure.Persistence;
using System.Threading.Tasks;

namespace RolexWatches.Infrastructure.Repository
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> NameExistsAsync(string name, int? excludeBrandId = null)
        {
            return await DbSet.AnyAsync(b =>
                b.Name == name && (excludeBrandId == null || b.Id != excludeBrandId));
        }

        public async Task<Brand?> GetByIdWithProductsAsync(int id)
        {
            return await DbSet
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}