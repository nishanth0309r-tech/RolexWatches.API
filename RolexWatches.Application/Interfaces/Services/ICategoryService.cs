using RolexWatches.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.Interfaces.Services
{
    
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto dto);
        Task<CategoryDto?> UpdateAsync(int id, CreateUpdateCategoryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
