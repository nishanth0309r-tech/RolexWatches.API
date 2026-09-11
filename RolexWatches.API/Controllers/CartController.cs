using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;

namespace RolexWatches.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service) => _service = service;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> GetCart() => Ok(await _service.GetCartAsync(UserId));

        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartDto dto) =>
            Ok(await _service.AddToCartAsync(UserId, dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuantity(int id, UpdateCartQuantityDto dto)
        {
            var result = await _service.UpdateQuantityAsync(UserId, id, dto.Quantity);
            return result ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _service.RemoveAsync(UserId, id);
            return result ? Ok() : NotFound();
        }
    }
}