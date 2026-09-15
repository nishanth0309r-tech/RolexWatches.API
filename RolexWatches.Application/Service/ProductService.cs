using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolexWatches.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repo;
        private readonly IMapper mapper;

        public ProductService(IProductRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await repo.GetAllAsync();
            return mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await repo.GetByIdAsync(id);
            return product == null ? null : mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var entity = mapper.Map<Product>(dto);
            await repo.AddAsync(entity);
            await repo.SaveChangesAsync();
            return mapper.Map<ProductDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) return false;

            mapper.Map(dto, entity);
            repo.Update(entity);
            return await repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) return false;

            repo.Delete(entity);
            return await repo.SaveChangesAsync();
        }
    }
}