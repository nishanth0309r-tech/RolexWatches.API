using Microsoft.EntityFrameworkCore;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;


namespace RolexWatches.Infrastructure.Repository
{
    public class ReviewRepository:IReviewRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ReviewRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(Review review)
        {
            await dbContext.Reviews.AddAsync(review);
        }

        public void Delete(Review review)
        {
           dbContext.Reviews.Remove(review);
        }

        public async Task<List<Review>> GetAllAsync()
        {
          return  await dbContext.Reviews.Include(r => r.Product)
                .Include(r => r.User).OrderByDescending(r => r.CreatedAt).ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
          return await dbContext.Reviews.Include(r => r.User).Include(r => r.Product)
            .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Review>> GetByProductIdAsync(int productId)
        {
            return await dbContext.Reviews.Include(r => r.User).Include(r => r.Product)
             .Where(r => r.ProductId == productId)
             .OrderByDescending(r => r.CreatedAt).ToListAsync();
        }

        public async Task<Review?> GetByUserAndProductAsync(int userId, int productId)
        {
            return await dbContext.Reviews.FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);
        }

        public async Task<bool> HasPurchasedAsync(int userId, int productId)
        {
            return await dbContext.Orders.AnyAsync(o => o.UserId == userId
           && o.Status != "Pending" && o.Status != "Cancelled"
           && o.OrderItems.Any(i => i.ProductId == productId));
        }

        public async Task<bool> SaveChangesAsync()
        {
          return await dbContext.SaveChangesAsync() > 0;
        }
    }
}
