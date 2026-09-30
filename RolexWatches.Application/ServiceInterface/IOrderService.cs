using RolexWatches.Application.Dto;

namespace RolexWatches.Application.ServiceInterface
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<bool> UpdateStatusAsync(int id, string status);
        Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto dto);
        Task<List<OrderDto>> GetByUserIdAsync(int userId);
        Task<OrderDto?> GetByIdAsync(int id, int userId);
    }
}