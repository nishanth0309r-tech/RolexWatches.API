using RolexWatches.Application.Dto;

namespace RolexWatches.Application.ServiceInterface
{
    public interface IWishlistService
    {
        Task<List<WishlistItemDto>> GetWishlistAsync(string userId);
        Task<bool> AddAsync(string userId, AddToWishlistDto dto);
        Task<bool> RemoveAsync(string userId, int productId);
    }
}