using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Infrastructure.Services;

namespace RolexWatches.API.Controllers
{
    [ApiController]
    [Route("api/admin/orders")]
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController : ControllerBase
    {
        private readonly IOrderService service;
        private readonly IPaymentService paymentService;

        public AdminOrdersController(IOrderService service,IPaymentService paymentService)
        {
            this.service = service;
            this.paymentService = paymentService;
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
            if (!result)
            {
                return NotFound();
            }

            // Cash is handed over when the parcel is delivered
            if (status.Equals("Delivered", StringComparison.OrdinalIgnoreCase))
            {
                await paymentService.MarkCodCollectedAsync(id);
            }

            return NoContent();
        }
    }
}
