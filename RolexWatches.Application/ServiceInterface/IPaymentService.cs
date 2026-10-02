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
    }
}
