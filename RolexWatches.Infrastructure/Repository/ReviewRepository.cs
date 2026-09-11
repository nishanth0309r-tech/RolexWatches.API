using Microsoft.EntityFrameworkCore;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Infrastructure.Repository
{
    public class ReviewRepository:IReviewRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ReviewRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Delete(Review review)
        {
           dbContext.Reviews.Remove(review);
        }

        public async Task<List<Review>> GetAllAsync()
        {
          return  await dbContext.Reviews.Include(r => r.Product)
                .Include(r => r.User).ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
          return await dbContext.Reviews.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
          return await dbContext.SaveChangesAsync() > 0;
        }
    }
}
