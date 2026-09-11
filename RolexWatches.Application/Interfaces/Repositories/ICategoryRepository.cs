using System.Collections.Generic;
using System.Threading.Tasks;
using RolexWatches.Domain.Entities;

namespace RolexWatches.Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> NameExistsAsync(string name, int? excludeCategoryId = null);
        Task<List<Category>> GetTopLevelWithChildrenAsync();
        Task<bool> HasProductsAsync(int categoryId);
    }
}