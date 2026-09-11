using Microsoft.EntityFrameworkCore;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using RolexWatches.Infrastructure.Data;

namespace RolexWatches.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext dbcontext;
        public OrderRepository(ApplicationDbContext context) => dbcontext = context;

        public async Task<List<Order>> GetAllAsync() =>
            await dbcontext.Orders.Include(o => o.User).Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product).ToListAsync();

        public async Task<Order?> GetByIdAsync(int id) =>
            await dbcontext.Orders.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == id);

        public void Update(Order order) => dbcontext.Orders.Update(order);

        public async Task<bool> SaveChangesAsync() => await dbcontext.SaveChangesAsync() > 0;
    }
}