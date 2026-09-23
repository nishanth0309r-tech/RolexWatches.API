using Microsoft.EntityFrameworkCore;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Enums;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolexWatches.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext dbcontext;

        public ProductRepository(ApplicationDbContext context)
        {
            dbcontext = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await dbcontext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Specifications)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await dbcontext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Specifications)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            await dbcontext.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            dbcontext.Products.Update(product);
        }

        public void Delete(Product product)
        {
            dbcontext.Products.Remove(product);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await dbcontext.SaveChangesAsync() > 0;
        }

        // ---------- Module 3: Search / Filter / Inventory ----------

        public async Task<(List<Product> Items, int TotalCount)> SearchAsync(
            string? query, int? brandId, int? categoryId,
            decimal? minPrice, decimal? maxPrice, bool? inStockOnly,
            ProductSortBy sortBy, int pageNumber, int pageSize)
        {
            var q = dbcontext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                q = q.Where(p => p.Name.Contains(term) || p.Description.Contains(term));
            }

            if (brandId.HasValue)
                q = q.Where(p => p.BrandId == brandId.Value);

            if (categoryId.HasValue)
                q = q.Where(p => p.CategoryId == categoryId.Value);

            if (minPrice.HasValue)
                q = q.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                q = q.Where(p => p.Price <= maxPrice.Value);

            if (inStockOnly == true)
                q = q.Where(p => p.StockQuantity > 0);

            q = sortBy switch
            {
                ProductSortBy.PriceLowToHigh => q.OrderBy(p => p.Price),
                ProductSortBy.PriceHighToLow => q.OrderByDescending(p => p.Price),
                ProductSortBy.NameAToZ => q.OrderBy(p => p.Name),
                ProductSortBy.NameZToA => q.OrderByDescending(p => p.Name),
                _ => q.OrderByDescending(p => p.CreatedAt)
            };

            var totalCount = await q.CountAsync();

            var items = await q
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<List<Product>> GetLowStockAsync()
        {
            return await dbcontext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.StockQuantity <= p.LowStockThreshold)
                .OrderBy(p => p.StockQuantity)
                .ToListAsync();
        }
    }
}