using RolexWatches.Application.Dto;

namespace RolexWatches.Application.ServiceInterface
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetSummaryAsync();
    }
}