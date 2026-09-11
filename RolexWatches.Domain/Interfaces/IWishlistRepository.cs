using System;
using System.Collections.Generic;
using System.Text;

using RolexWatches.Domain.Entities;

namespace RolexWatches.Domain.Interfaces
{
    public interface IWishlistRepository
    {
        Task<List<WishlistItem>> GetByUserIdAsync(string userId);
        Task<WishlistItem?> GetByUserAndProductAsync(string userId, int productId);
        Task AddAsync(WishlistItem item);
        void Remove(WishlistItem item);
        Task<bool> SaveChangesAsync();
    }
}