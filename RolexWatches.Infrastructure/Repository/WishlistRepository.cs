using Microsoft.EntityFrameworkCore;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;

namespace RolexWatches.Infrastructure.Repository
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly ApplicationDbContext _context;
        public WishlistRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<WishlistItem>> GetByUserIdAsync(string userId) =>
            await _context.WishlistItems.Include(w => w.Product)
                .Where(w => w.UserId == userId).ToListAsync();

        public async Task<WishlistItem?> GetByUserAndProductAsync(string userId, int productId) =>
            await _context.WishlistItems.FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

        public async Task AddAsync(WishlistItem item) => await _context.WishlistItems.AddAsync(item);
        public void Remove(WishlistItem item) => _context.WishlistItems.Remove(item);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

        Task<List<WishlistItem>> IWishlistRepository.GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        Task<WishlistItem?> IWishlistRepository.GetByUserAndProductAsync(string userId, int productId)
        {
            throw new NotImplementedException();
        }

       
    }
}