using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.ServiceInterface;

namespace RolexWatches.API.Controllers
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService service;

        public DashboardController(IDashboardService service)
        {
            this.service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await service.GetSummaryAsync();
            return Ok(summary);
        }
    }
}
