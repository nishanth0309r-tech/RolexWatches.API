using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.ServiceInterface;

namespace RolexWatches.API.Controllers
{
    [ApiController]
    [Route("api/admin/reviews")]
    [Authorize(Roles = "Admin")]
    public class AdminReviewsController : ControllerBase
    {
        private readonly IReviewService service;

        public AdminReviewsController(IReviewService service)
        {
            this.service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await service.GetAllAsync();
            return Ok(reviews);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var result = await service.DeleteAsync(id);
            if (result)
            {
                return Ok(new { message = "Review deleted successfully." });
            }
            else
            {
                return NotFound(new { message = "Review not found." });
            }
        }
    }
}
