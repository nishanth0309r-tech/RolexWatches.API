using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using System.Security.Claims;

namespace RolexWatches.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay(PayRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await paymentService.PayAsync(userId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
