using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.Service
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository repo;
        private readonly IMapper mapper;
        private readonly ICartService cartService;

        public OrderService(IOrderRepository repo, IMapper mapper,ICartService cartService) {
            this.repo = repo;
            this.mapper = mapper;
            this.cartService = cartService;
        }

        public async Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ShippingAddress))
                throw new InvalidOperationException("Shipping address is required.");

            var allCartItems = await cartService.GetCartAsync(userId.ToString());

            if (allCartItems == null || allCartItems.Count == 0)
                throw new InvalidOperationException("Your cart is empty.");

            // Order only the products the user selected; no selection means the whole cart.
            var requestedIds = (dto.Items ?? new()).Select(i => i.ProductId).ToHashSet();
            var cartItems = requestedIds.Count > 0
                ? allCartItems.Where(ci => requestedIds.Contains(ci.ProductId)).ToList()
                : allCartItems.ToList();

            if (cartItems.Count == 0)
                throw new InvalidOperationException("The selected items are not in your cart.");

            var order = new Order
            {
                UserId = userId,
                ShippingAddress = dto.ShippingAddress.Trim(),
                Status = Domain.Enums.OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                OrderItems = cartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,     // taken from the cart in the DB, not from the request
                    UnitPrice = ci.Price        // price snapshot, so order history survives price changes
                }).ToList()
            };

            order.TotalAmount = order.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity);

            await repo.AddAsync(order);
            if (!await repo.SaveChangesAsync())
                throw new InvalidOperationException("Failed to save order.");

            // Remove only the items that were ordered; the rest stay in the cart.
            foreach (var item in cartItems)
                await cartService.RemoveAsync(userId.ToString(), item.Id);

            // Re-fetch with includes so the DTO's Items list is populated.
            var savedOrder = await repo.GetByIdAsync(order.Id);
            return mapper.Map<OrderDto>(savedOrder);
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            var orders=await repo.GetAllAsync();
            return mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto?> GetByIdAsync(int id, int userId)
        {
            var order = await repo.GetByIdAsync(id);
            if (order == null || order.UserId != userId) return null; // ownership check

            return mapper.Map<OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetByUserIdAsync(int userId)
        {
            var orders = await repo.GetByUserIdAsync(userId);
            return mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            if (!Enum.TryParse<Domain.Enums.OrderStatus>(status, ignoreCase: true, out var newStatus)
                || !Enum.IsDefined(newStatus))
                return false;

            var order = await repo.GetByIdAsync(id);
            if (order == null)
            {
                return false;
            }
            order.Status = newStatus;
            repo.Update(order);
            return await repo.SaveChangesAsync();
        }
    }
}
