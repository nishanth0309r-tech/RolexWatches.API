using Microsoft.EntityFrameworkCore;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Infrastructure.Data;


namespace RolexWatches.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext dbcontext;
        public DashboardService(ApplicationDbContext context) => dbcontext = context;

        public async Task<DashboardDto> GetSummaryAsync()
        {
            return new DashboardDto
            {
                TotalProducts = await dbcontext.Products.CountAsync(),
                TotalOrders = await dbcontext.Orders.CountAsync(),
                TotalCustomers = await dbcontext.Users.CountAsync(u => u.Role == "Customer"),
                TotalRevenue = await dbcontext.Orders.SumAsync(o => (decimal?)o.TotalAmount) ?? 0,
                RecentOrders = await dbcontext.Orders
                    .Include(o => o.User)
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(5)
                    .Select(o => new RecentOrderDto
                    {
                        OrderId = o.Id,
                        CustomerName = o.User!.FullName,
                        Total = o.TotalAmount,
                        Status = o.Status,
                        CreatedAt = o.CreatedAt
                    }).ToListAsync()
            };
        }
    }
}