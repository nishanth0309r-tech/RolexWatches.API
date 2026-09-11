using RolexWatches.Domain.Entities;

namespace RolexWatches.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllCustomersAsync();
        Task<User?> GetByIdAsync(int id);
        void Update(User user);
        Task<bool> SaveChangesAsync();
    }
}