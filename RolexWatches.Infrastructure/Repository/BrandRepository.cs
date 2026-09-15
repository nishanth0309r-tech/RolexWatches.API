using Microsoft.EntityFrameworkCore;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolexWatches.Infrastructure.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext dbcontext;

        public BrandRepository(ApplicationDbContext context)
        {
            dbcontext = context;
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await dbcontext.Brands.Include(b => b.Products).ToListAsync();
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await dbcontext.Brands
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Brand brand)
        {
            await dbcontext.Brands.AddAsync(brand);
        }

        public void Update(Brand brand)
        {
            dbcontext.Brands.Update(brand);
        }

        public void Delete(Brand brand)
        {
            dbcontext.Brands.Remove(brand);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await dbcontext.SaveChangesAsync() > 0;
        }
    }
}