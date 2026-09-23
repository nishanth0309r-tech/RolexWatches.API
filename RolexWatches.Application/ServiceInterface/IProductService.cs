using System.Collections.Generic;
using System.Threading.Tasks;
using RolexWatches.Application.Dto;

namespace RolexWatches.Application.ServiceInterface
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto dto);
        Task<bool> UpdateAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteAsync(int id);

        // ---------- Module 3: Search / Filter / Inventory ----------
        Task<PagedResult<ProductDto>> SearchAsync(ProductFilterDto filter);
        Task<bool> UpdateStockAsync(int id, int newStockQuantity);
        Task<List<ProductDto>> GetLowStockAsync();
    }
}
