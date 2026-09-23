using Microsoft.EntityFrameworkCore;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;

namespace RolexWatches.Infrastructure.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext dbcontext;

        public CartRepository(ApplicationDbContext context)
        {
            dbcontext = context;
        }

        public async Task<List<CartItem>> GetByUserIdAsync(string userId)
        {
            return await dbcontext.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<CartItem?> GetByUserAndProductAsync(
            string userId,
            int productId)
        {
            return await dbcontext.CartItems
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c =>
                    c.UserId == userId &&
                    c.ProductId == productId);
        }

        public async Task<CartItem?> GetByIdAsync(int id)
        {
            return await dbcontext.CartItems
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(CartItem item)
        {
            await dbcontext.CartItems.AddAsync(item);
        }

        public void Update(CartItem item)
        {
            dbcontext.CartItems.Update(item);
        }

        public void Remove(CartItem item)
        {
            dbcontext.CartItems.Remove(item);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await dbcontext.SaveChangesAsync() > 0;
        }
    }
}