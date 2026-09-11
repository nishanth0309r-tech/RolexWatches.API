using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.ServiceInterface;

namespace RolexWatches.API.Controllers
{
    [ApiController]
    [Route("api/admin/customers")]
    [Authorize(Roles = "Admin")]
    public class AdminCustomersController : ControllerBase
    {
        private readonly ICustomerService service;

        public AdminCustomersController(ICustomerService service)
        {
            this.service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await service.GetAllAsync();
            return Ok(customers);
        }
        [HttpPut("{id}/block")]
        public async Task<IActionResult> BlockCustomer(int id)
        {
            var result = await service.ToggleBlockAsync(id);
            if (result)
            {
                return Ok(new { message = "Customer blocked successfully." });
            }
            else
            {
                return NotFound(new { message = "Customer not found." });
            }
        }
    }
}
