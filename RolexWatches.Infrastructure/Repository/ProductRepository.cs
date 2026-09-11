using Microsoft.EntityFrameworkCore;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Infrastructure.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly ApplicationDbContext dbcontext;

        public ProductRepository(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task AddAsync(Product product)
        {
            await dbcontext.Products.AddAsync(product);
        }

        public void Delete(Product product)
        {
            dbcontext.Products.Remove(product);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await dbcontext.Products.Include(p => p.Brand).Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
           return await dbcontext.Products.Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await dbcontext.SaveChangesAsync() > 0;
        }

        public void Update(Product product)
        {
          dbcontext.Products.Update(product);  
        }
    }
}
