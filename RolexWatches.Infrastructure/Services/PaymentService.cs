using Microsoft.EntityFrameworkCore;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Entities;
using RolexWatches.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolexWatches.Application.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentGateway Gateway;
        private readonly ApplicationDbContext dbContext;

        public PaymentService(IPaymentGateway Gateway, ApplicationDbContext dbContext)
        {
            this.Gateway = Gateway;
            this.dbContext = dbContext;
        }
        public async Task<PaymentResultDto> PayAsync(string userId, PayRequestDto dto)
        {
            var order = await dbContext.Orders
            .Include(o => o.OrderItems).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == dto.OrderId && o.UserId == int.Parse(userId));

            if (order is null) return new(false, null, "Order not found");
            if (order.Status != "Pending") return new(false, null, "Order is not awaiting payment");

            var payment = new Payment { OrderId = order.Id, Amount = order.TotalAmount, Method = dto.Method };
            dbContext.PaymentsQ9.Add(payment);

            var (ok, txnId) = await Gateway.ChargeAsync(order.TotalAmount, dto.Method, dto.CardNumber);
            payment.TransactionId = txnId;
            payment.Status = ok ? "Succeeded" : "Failed";

            if (ok)
            {
                order.Status = "Processing";
                // reduce stock (re-check it here to avoid overselling)
                // clear the user's cart here too
            }
            await dbContext.SaveChangesAsync();
            return new(ok, txnId, ok ? "Payment successful" : "Payment failed");
        }
    }
}
