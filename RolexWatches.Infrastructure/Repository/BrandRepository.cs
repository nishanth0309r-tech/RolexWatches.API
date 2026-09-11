using Microsoft.EntityFrameworkCore;
using RolexWatches.Application.Interfaces.Repositories;
using RolexWatches.Domain.Entities;
using RolexWatches.Infrastructure.Persistence;
using System.Threading.Tasks;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Infrastructure.Repository
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext dbcontext;
        public BrandRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> NameExistsAsync(string name, int? excludeBrandId = null)
        {
            this.dbcontext = dbcontext;
            return await DbSet.AnyAsync(b =>
                b.Name == name && (excludeBrandId == null || b.Id != excludeBrandId));
        }

        public async Task<Brand?> GetByIdWithProductsAsync(int id)
        public async Task AddAsync(Brand brand)
        {
            await dbcontext.Brands.AddAsync(brand); 
        }

        public void DeleteAsync(Brand brand)
        {
            dbcontext.Brands.Remove(brand);
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await dbcontext.Brands.ToListAsync();
        }

        public async Task<Brand> GetByIdAsync(int id)
        {
            return await dbcontext.Brands.FindAsync(id);    
        }

        public async Task<bool> SaveChangesAsync()
        {
           return await dbcontext.SaveChangesAsync() > 0;
        }

        public void Update(Brand brand)
        {
            dbcontext.Brands.Update(brand);
            return await DbSet
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
