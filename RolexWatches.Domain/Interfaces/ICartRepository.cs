using System;
using System.Collections.Generic;
using System.Text;

using RolexWatches.Domain.Entities;

namespace RolexWatches.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<List<CartItem>> GetByUserIdAsync(string userId);
        Task<CartItem?> GetByUserAndProductAsync(string userId, int productId);
        Task<CartItem?> GetByIdAsync(int id);
        Task AddAsync(CartItem item);
        void Update(CartItem item);
        void Remove(CartItem item);
        Task<bool> SaveChangesAsync();
    }
}