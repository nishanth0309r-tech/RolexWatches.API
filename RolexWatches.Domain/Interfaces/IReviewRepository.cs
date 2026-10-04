using RolexWatches.Domain.Entities;

namespace RolexWatches.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetByProductIdAsync(int productId);
        Task<List<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<Review?> GetByUserAndProductAsync(int userId, int productId);
        Task<bool> HasPurchasedAsync(int userId, int productId);
        Task AddAsync(Review review);
        void Delete(Review review);
        Task<bool> SaveChangesAsync();
    }
}