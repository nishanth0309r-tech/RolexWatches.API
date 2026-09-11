using RolexWatches.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Domain.Interfaces
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetAllAsync();
        Task<Brand>GetByIdAsync(int id);
        Task AddAsync(Brand brand);
        void Update(Brand brand);
        void DeleteAsync(Brand brand);
        Task<bool> SaveChangesAsync();


    }
}
