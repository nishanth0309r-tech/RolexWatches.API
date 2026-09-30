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
            // userId param kept for interface symmetry with GetByUserIdAsync/GetByIdAsync,
            // but the cart is looked up by the string form the claim actually provides —
            // see the ClaimTypes.NameIdentifier note below.
            var cartItems = await cartService.GetCartAsync(userId.ToString());

            if (cartItems == null || cartItems.Count == 0)
                throw new InvalidOperationException("Your cart is empty.");

            var order = new Order
            {
                UserId = userId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                OrderItems = cartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Price   // snapshot — protects order history if price changes later
                }).ToList()
            };

            order.TotalAmount = order.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity);

            await repo.AddAsync(order);
            var saved = await repo.SaveChangesAsync();
            if (!saved)
                throw new InvalidOperationException("Failed to save order.");

            // clear the cart now that it's been converted to an order
            foreach (var item in cartItems)
                await cartService.RemoveAsync(userId.ToString(), item.Id);

            // re-fetch with includes so the DTO's Items list is populated
            // (order.OrderItems here has no Product navigation loaded yet)
            var saved_order = await repo.GetByIdAsync(order.Id);
            return mapper.Map<OrderDto>(saved_order);
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
            var order = await repo.GetByIdAsync(id);
            if (order == null)
            {
                return false;
            }
            order.Status = status;
            repo.Update(order);
            return await repo.SaveChangesAsync();
        }
    }
}
