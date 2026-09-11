using RolexWatches.Domain.Entities;

namespace RolexWatches.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        void Update(Order order);
        Task<bool> SaveChangesAsync();
    }
}