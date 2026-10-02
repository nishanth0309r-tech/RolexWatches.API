using RolexWatches.Application.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolexWatches.Infrastructure.Services
{
    public class PaymentGateway : IPaymentGateway
    {
        public Task<(bool Success, string TransactionId)> ChargeAsync(decimal amount, string method, string? card)
        {
            // test rule: cards ending in 0000 fail, everything else succeeds
            var ok = !(card?.EndsWith("0000") ?? false);
            return Task.FromResult((ok, $"TXN-{Guid.NewGuid():N}"[..16]));
        }
    }
}
