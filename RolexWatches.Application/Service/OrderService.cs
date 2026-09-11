using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
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

        public OrderService(IOrderRepository repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            var orders=await repo.GetAllAsync();
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
