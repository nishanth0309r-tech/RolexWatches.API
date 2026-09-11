using Microsoft.EntityFrameworkCore;
using RolexWatches.Application.DTOs.Product;
using RolexWatches.Application.Interfaces.Repositories;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Enums;
using RolexWatches.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolexWatches.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Product?> GetByIdWithDetailsAsync(int id)
        {
            return await DbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Specifications)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> SkuExistsAsync(string sku, int? excludeProductId = null)
        {
            return await DbSet.AnyAsync(p =>
                p.Sku == sku && (excludeProductId == null || p.Id != excludeProductId));
        }

        public async Task<(List<Product> Items, int TotalCount)> SearchAsync(ProductFilterDto filter)
        {
            var query = DbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Query))
            {
                var term = filter.Query.Trim();
                query = query.Where(p =>
                    p.Name.Contains(term) || p.Description.Contains(term));
            }

            if (filter.BrandId.HasValue)
                query = query.Where(p => p.BrandId == filter.BrandId.Value);

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            if (filter.InStockOnly == true)
                query = query.Where(p => p.StockQuantity > 0);

            query = filter.SortBy switch
            {
                ProductSortBy.PriceLowToHigh => query.OrderBy(p => p.Price),
                ProductSortBy.PriceHighToLow => query.OrderByDescending(p => p.Price),
                ProductSortBy.NameAToZ => query.OrderBy(p => p.Name),
                ProductSortBy.NameZToA => query.OrderByDescending(p => p.Name),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<List<Product>> GetLowStockProductsAsync()
        {
            return await DbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.StockQuantity <= p.LowStockThreshold)
                .OrderBy(p => p.StockQuantity)
                .ToListAsync();
        }
    }
}