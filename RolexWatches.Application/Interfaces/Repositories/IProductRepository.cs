using System.Collections.Generic;
using System.Threading.Tasks;
using RolexWatches.Application.DTOs.Product;
using RolexWatches.Domain.Entities;

namespace RolexWatches.Application.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetByIdWithDetailsAsync(int id);
        Task<bool> SkuExistsAsync(string sku, int? excludeProductId = null);
        Task<(List<Product> Items, int TotalCount)> SearchAsync(ProductFilterDto filter);
        Task<List<Product>> GetLowStockProductsAsync();
    }
}