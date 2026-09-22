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
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _service;
        public WishlistController(IWishlistService service) => _service = service;

        //private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        private string UserId =>
    User.FindFirstValue(ClaimTypes.NameIdentifier)
    ?? throw new UnauthorizedAccessException("Token missing user identifier.");

        [HttpGet]
        public async Task<IActionResult> GetWishlist() => Ok(await _service.GetWishlistAsync(UserId));

        [HttpPost]
        public async Task<IActionResult> Add(AddToWishlistDto dto)
        {
           return Ok(await _service.AddAsync(UserId, dto));
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            var result = await _service.RemoveAsync(UserId, productId);
            return result ? Ok() : NotFound();
        }
    }
}