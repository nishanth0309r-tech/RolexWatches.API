using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolexWatches.Application.Dto
{
    public record PaymentResultDto(bool Success, string? TransactionId, string Message);
    
}
