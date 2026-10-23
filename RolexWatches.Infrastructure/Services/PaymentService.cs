using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Enums;
using RolexWatches.Infrastructure.Data;
using Stripe;
using Stripe.Checkout;

namespace RolexWatches.Infrastructure.Services;

public class PaymentService(IPaymentGateway gateway, ApplicationDbContext dbContext, IConfiguration config) : IPaymentService
{
    public async Task<PaymentResultDto> ConfirmCashOnDeliveryAsync(string userId, int orderId)
    {
        if (!int.TryParse(userId, out var uid))
            return new(false, null, "Invalid user");

        var order = await dbContext.Orders
            .Include(o => o.OrderItems).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == uid);

        if (order is null) return new(false, null, "Order not found");
        if (order.Status != OrderStatus.Pending) return new(false, null, "Order is not awaiting payment");

        // optional cap, set "Cod": { "MaxAmount": 50000 } in appsettings.json
        if (decimal.TryParse(config["Cod:MaxAmount"], System.Globalization.NumberStyles.Number,
             System.Globalization.CultureInfo.InvariantCulture, out var maxCod)
         && order.TotalAmount > maxCod)
        {
            return new(false, null, $"Cash on delivery is not available for orders above {maxCod:N0}");
        }

        foreach (var item in order.OrderItems)
        {
            if (item.Product is null || !item.Product.IsActive)
                return new(false, null, "A product in this order is no longer available");
            if (item.Product.StockQuantity < item.Quantity)
                return new(false, null, $"Insufficient stock for {item.Product.Name}");
        }

        // If the customer already opened a Stripe page for this order, close it so it can't be paid later
        var stripePayments = await dbContext.PaymentsQ9
            .Where(p => p.OrderId == orderId && p.Method == "Stripe" && p.Status == "Pending")
            .ToListAsync();

        var sessionService = new SessionService();
        foreach (var p in stripePayments)
        {
            var s = await sessionService.GetAsync(p.TransactionId);
            if (s.Status == "complete")
                return new(false, null, "A card payment for this order was already completed. Refresh My Orders.");
            if (s.Status == "open")
                await sessionService.ExpireAsync(s.Id);
            p.Status = "Expired";
        }

        await using var tx = await dbContext.Database.BeginTransactionAsync();

        var claimed = await dbContext.Orders
            .Where(o => o.Id == orderId && o.Status == OrderStatus.Pending)
            .ExecuteUpdateAsync(s => s.SetProperty(o => o.Status, OrderStatus.Processing));
        if (claimed == 0) return new(false, null, "Order was already processed");

        foreach (var item in order.OrderItems)
            item.Product!.StockQuantity -= item.Quantity;   // null ruled out above

        dbContext.PaymentsQ9.Add(new Payment
        {
            OrderId = order.Id,
            Amount = order.TotalAmount,
            Method = "COD",
            TransactionId = $"COD-{order.Id}",
            Status = "Pending"          // becomes Succeeded when the cash is collected
        });

        await dbContext.SaveChangesAsync();
        await tx.CommitAsync();
        return new(true, null, "Order confirmed. Pay in cash on delivery.");
    }

    public async Task<string> CreateCheckoutSessionAsync(string userId, int orderId)
    {
        if (!int.TryParse(userId, out var uid))
            throw new UnauthorizedAccessException();

        var order = await dbContext.Orders
            .Include(o => o.OrderItems).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == uid)
            ?? throw new KeyNotFoundException("Order not found");

        if (order.Status != OrderStatus.Pending)
            throw new InvalidOperationException("Order is not awaiting payment");

        var baseUrl = config["Stripe:FrontendBaseUrl"];

        var options = new SessionCreateOptions
        {
            Mode = "payment",
            ClientReferenceId = order.Id.ToString(),
            Metadata = new Dictionary<string, string> { ["orderId"] = order.Id.ToString() },
            SuccessUrl = $"{baseUrl}/order-success?orderId={order.Id}",
            CancelUrl = $"{baseUrl}/orders",
            LineItems = order.OrderItems.Select(i => new SessionLineItemOptions
            {
                Quantity = i.Quantity,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "inr",
                    UnitAmount = (long)(i.UnitPrice * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = i.Product?.Name ?? $"Product {i.ProductId}"
                    }
                }
            }).ToList()
        };

        var session = await new SessionService().CreateAsync(options);

        dbContext.PaymentsQ9.Add(new Payment
        {
            OrderId = order.Id,
            Amount = order.TotalAmount,
            Method = "Stripe",
            TransactionId = session.Id,      // replaced with the PaymentIntent id on success
            Status = "Pending"
        });
        await dbContext.SaveChangesAsync();

        return session.Url;
    }



    public async Task HandleWebhookAsync(string json, string stripeSignature)
    {
        var stripeEvent = EventUtility.ConstructEvent(
            json, stripeSignature, config["Stripe:WebhookSecret"],
            throwOnApiVersionMismatch: false);

        if (stripeEvent.Type != "checkout.session.completed") return;

        var session = (Session)stripeEvent.Data.Object;
        if (session.PaymentStatus != "paid") return;
        if (!int.TryParse(session.ClientReferenceId, out var orderId)) return;

        await FulfillOrderAsync(orderId, session.Id, session.PaymentIntentId);
    }

    public async Task MarkCodCollectedAsync(int orderId)
    {
        await dbContext.PaymentsQ9
            .Where(p => p.OrderId == orderId && p.Method == "COD" && p.Status == "Pending")
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.Status, "Succeeded"));
    }

    public async Task<PaymentResultDto> PayAsync(string userId, PayRequestDto dto)
    {
        if (!int.TryParse(userId, out var uid))
            return new(false, null, "Invalid user");

        var order = await dbContext.Orders
            .Include(o => o.OrderItems).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == dto.OrderId && o.UserId == uid);

        if (order is null) return new(false, null, "Order not found");
        if (order.Status != OrderStatus.Pending) return new(false, null, "Order is not awaiting payment");

        // check stock BEFORE charging the customer
        foreach (var item in order.OrderItems)
        {
            if (item.Product is null)
                return new(false, null, "Product no longer exists");

            if (!item.Product.IsActive)
                return new(false, null, $"{item.Product.Name} is no longer available");

            if (item.Product.StockQuantity < item.Quantity)
                return new(false, null, $"Insufficient stock for {item.Product.Name}");
        }

        var payment = new Payment { OrderId = order.Id, Amount = order.TotalAmount, Method = dto.Method };
        dbContext.PaymentsQ9.Add(payment);

        var (ok, txnId) = await gateway.ChargeAsync(order.TotalAmount, dto.Method, dto.CardNumber);
        payment.TransactionId = txnId;
        payment.Status = ok ? "Succeeded" : "Failed";

        if (ok)
        {
            foreach (var item in order.OrderItems)
                item.Product!.StockQuantity -= item.Quantity;   // null already ruled out above



            order.Status = OrderStatus.Processing;
        }
        else
        {
            order.Status = OrderStatus.Failed;   // or leave as Pending to allow a retry
        }

        await dbContext.SaveChangesAsync();
        return new(ok, txnId, ok ? "Payment successful" : "Payment failed");
    }

    public async Task<PaymentResultDto> VerifyPaymentAsync(string userId, int orderId)
    {
        if (!int.TryParse(userId, out var uid))
            return new(false, null, "Invalid user");

        var order = await dbContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == uid);

        if (order is null) return new(false, null, "Order not found");

        // Already confirmed (the webhook got there first)
        if (order.Status != OrderStatus.Pending)
            return new(order.Status != OrderStatus.Failed && order.Status != OrderStatus.Cancelled,
                       null, $"Order status: {order.Status}");

        // Still Pending: ask Stripe directly about this order's checkout sessions
        var pendingPayments = await dbContext.PaymentsQ9
            .Where(p => p.OrderId == orderId && p.Method == "Stripe" && p.Status == "Pending")
            .ToListAsync();

        var sessionService = new SessionService();
        foreach (var payment in pendingPayments)
        {
            var session = await sessionService.GetAsync(payment.TransactionId);
            if (session.PaymentStatus == "paid")
            {
                await FulfillOrderAsync(orderId, session.Id, session.PaymentIntentId);
                return new(true, session.PaymentIntentId, "Payment confirmed");
            }
        }

        return new(false, null, "Payment not completed yet");
    }

    private async Task<bool> FulfillOrderAsync(int orderId, string sessionId, string? paymentIntentId)
    {
        await using var tx = await dbContext.Database.BeginTransactionAsync();

        // Atomic claim: only one caller can flip Pending -> Processing
        var claimed = await dbContext.Orders
            .Where(o => o.Id == orderId && o.Status == OrderStatus.Pending)
            .ExecuteUpdateAsync(s => s.SetProperty(o => o.Status, OrderStatus.Processing));

        if (claimed == 0) return false;   // already handled by the webhook or a previous verify

        var items = await dbContext.OrderItems
            .Include(i => i.Product)
            .Where(i => i.OrderId == orderId)
            .ToListAsync();

        foreach (var item in items)
        {
            if (item.Product is not null)
                item.Product.StockQuantity -= item.Quantity;
        }

        var payment = await dbContext.PaymentsQ9.FirstOrDefaultAsync(p => p.TransactionId == sessionId);
        if (payment is not null)
        {
            payment.Status = "Succeeded";
            if (paymentIntentId is not null) payment.TransactionId = paymentIntentId;
        }

        await dbContext.SaveChangesAsync();
        await tx.CommitAsync();
        return true;
    }
}