using RolexWatches.Application.Dto;

namespace RolexWatches.Application.ServiceInterface
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<bool> UpdateStatusAsync(int id, string status);
    }
}