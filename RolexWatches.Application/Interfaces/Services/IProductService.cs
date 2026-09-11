using RolexWatches.Application.DTOs.Common;
using RolexWatches.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.Interfaces.Services
{
   
    public interface IProductService
    {
        Task<ProductDto?> GetByIdAsync(int id);
        Task<PagedResult<ProductDto>> SearchAsync(ProductFilterDto filter);
        Task<ProductDto> CreateAsync(CreateProductDto dto);
        Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteAsync(int id);

        Task<ProductDto?> UpdateStockAsync(int id, int newStockQuantity);
        Task<PagedResult<ProductDto>> GetLowStockAsync(int pageNumber, int pageSize);
    }
}