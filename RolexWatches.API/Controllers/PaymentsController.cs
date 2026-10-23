using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Entities;
using System.Security.Claims;

namespace RolexWatches.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IPaymentService paymentService) : ControllerBase
    {
        [Authorize]
        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> Create([FromBody] CreateSessionRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            try
            {
                var url = await paymentService.CreateCheckoutSessionAsync(userId, req.OrderId);
                return Ok(new { url });
            }
            catch (KeyNotFoundException) { return NotFound(new { message = "Order not found" }); }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [Authorize]
        [HttpPost("verify/{orderId:int}")]
        public async Task<IActionResult> Verify(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await paymentService.VerifyPaymentAsync(userId, orderId));
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            try
            {
                await paymentService.HandleWebhookAsync(json, Request.Headers["Stripe-Signature"]!);
                return Ok();
            }
            catch (Stripe.StripeException) { return BadRequest(); }
        }


        [Authorize]
        [HttpPost("cod/{orderId:int}")]
        public async Task<IActionResult> Cod(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await paymentService.ConfirmCashOnDeliveryAsync(userId, orderId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }



    public record CreateSessionRequest(int OrderId);
}
