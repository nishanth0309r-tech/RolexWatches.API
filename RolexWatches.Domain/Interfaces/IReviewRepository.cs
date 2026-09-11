using RolexWatches.Domain.Entities;

namespace RolexWatches.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        void Delete(Review review);
        Task<bool> SaveChangesAsync();
    }
}