using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using System.Security.Claims;

namespace RolexWatches.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> Checkout(CreateOrderDto dto)
        {
            try
            {
                return Ok(await orderService.CreateOrderAsync(UserId, dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders() =>
            Ok(await orderService.GetByUserIdAsync(UserId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await orderService.GetByIdAsync(id, UserId);
            return order == null ? NotFound() : Ok(order);
        }
    }
}
