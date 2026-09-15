using RolexWatches.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolexWatches.Application.ServiceInterface
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetAllAsync();
        Task<BrandDto?> GetByIdAsync(int id);
        Task<BrandDto> CreateAsync(CreateBrandDto dto);
        Task<bool> UpdateAsync(int id, CreateBrandDto dto);
        Task<bool> DeleteAsync(int id);
    }
}