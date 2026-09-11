using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.ServiceInterface;

namespace RolexWatches.API.Controllers
{
    [ApiController]
    [Route("api/admin/orders")]
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController : ControllerBase
    {
        private readonly IOrderService service;

        public AdminOrdersController(IOrderService service)
        {
            this.service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await service.GetAllAsync();
            return Ok(orders);
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            var result = await service.UpdateStatusAsync(id, status);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
