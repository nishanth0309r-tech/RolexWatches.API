using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using System.Security.Claims;

namespace RolexWatches.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetForProduct(int productId)
        {
            return Ok(await reviewService.GetForProductAsync(productId));
        }
            
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
        {
            try { return Ok(await reviewService.CreateAsync(UserId, dto)); }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        }
    }
}
