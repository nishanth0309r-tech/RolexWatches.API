using RolexWatches.Application.Dto;

namespace RolexWatches.Application.ServiceInterface
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}