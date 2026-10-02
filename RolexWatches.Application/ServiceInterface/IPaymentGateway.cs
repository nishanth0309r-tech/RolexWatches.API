using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolexWatches.Application.ServiceInterface
{
    public interface IPaymentGateway
    {
        Task<(bool Success, string TransactionId)> ChargeAsync(decimal amount, string method, string? card);
    }
}
