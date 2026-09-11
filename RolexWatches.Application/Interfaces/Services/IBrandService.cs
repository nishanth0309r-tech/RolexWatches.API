using RolexWatches.Application.DTOs.Brand;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.Interfaces.Services
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetAllAsync();
        Task<BrandDto?> GetByIdAsync(int id);
        Task<BrandDto> CreateAsync(CreateUpdateBrandDto dto);
        Task<BrandDto?> UpdateAsync(int id, CreateUpdateBrandDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
