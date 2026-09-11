using Microsoft.EntityFrameworkCore;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Infrastructure.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext dbcontext;

        public BrandRepository(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

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
        }
    }
}
