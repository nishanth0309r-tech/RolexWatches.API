using RolexWatches.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolexWatches.Application.ServiceInterface
{
    public interface IPaymentService
    {
        Task<PaymentResultDto> PayAsync(string userId, PayRequestDto dto);
        Task<string> CreateCheckoutSessionAsync( string userId, int orderId);
        Task HandleWebhookAsync(string json, string stripeSignature);
        Task<PaymentResultDto> VerifyPaymentAsync(string userId, int orderId);
        Task<PaymentResultDto> ConfirmCashOnDeliveryAsync(string userId, int orderId);
        Task MarkCodCollectedAsync(int orderId);
    }
}
