using System.Collections.Generic;
using System.Threading.Tasks;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Enums;

namespace RolexWatches.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<bool> SaveChangesAsync();

        // ---------- Module 3: Search / Filter / Inventory ----------
        // Plain parameters only — Domain layer must not depend on Application DTOs
        Task<(List<Product> Items, int TotalCount)> SearchAsync(
            string? query, int? brandId, int? categoryId,
            decimal? minPrice, decimal? maxPrice, bool? inStockOnly,
            ProductSortBy sortBy, int pageNumber, int pageSize);

        Task<List<Product>> GetLowStockAsync();
    }
}