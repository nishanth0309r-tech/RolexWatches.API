using RolexWatches.Application.Dto;

namespace RolexWatches.Application.ServiceInterface
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync();
        Task<bool> ToggleBlockAsync(int id);
    }
}