using RolexWatches.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolexWatches.Domain.Interfaces
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(int id);
        Task AddAsync(Brand brand);
        void Update(Brand brand);
        void Delete(Brand brand);
        Task<bool> SaveChangesAsync();
    }
}