using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly IUserRepository repo;
        private readonly IMapper mapper;

        public CustomerService(IUserRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
           var users=await repo.GetAllCustomersAsync();
            return mapper.Map<List<CustomerDto>>(users);
        }

        public async Task<bool> ToggleBlockAsync(int id)
        {
            var user =await repo.GetByIdAsync(id);
            if(user == null)
            {
                return false;
            }
            user.IsActive=!user.IsActive;
            repo.Update(user);
            return await repo.SaveChangesAsync();
        }
    }
}
