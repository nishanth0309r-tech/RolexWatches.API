using RolexWatches.Application.Dto;

namespace RolexWatches.Application.ServiceInterface
{
    public interface IReviewService
    {
        Task<ProductReviewsDto> GetForProductAsync(int productId);
        Task<List<ReviewDto>> GetAllAsync();
        Task<ReviewDto> CreateAsync(int userId, CreateReviewDto dto);
        Task<bool> DeleteAsync(int id);
    }
}