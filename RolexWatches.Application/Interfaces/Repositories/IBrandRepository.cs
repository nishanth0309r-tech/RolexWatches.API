using RolexWatches.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.Interfaces.Repositories
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<bool> NameExistsAsync(string name, int? excludeBrandId = null);
        Task<Brand?> GetByIdWithProductsAsync(int id);
    }
}

