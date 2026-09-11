using RolexWatches.Application.Dto;

namespace RolexWatches.Application.ServiceInterface
{
    public interface ICartService
    {
        Task<List<CartItemDto>> GetCartAsync(string userId);
        Task<CartItemDto> AddToCartAsync(string userId, AddToCartDto dto);
        Task<bool> UpdateQuantityAsync(string userId, int cartItemId, int quantity);
        Task<bool> RemoveAsync(string userId, int cartItemId);
    }
}