using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolexWatches.Application.Service
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository repo;
        private readonly IMapper mapper;

        public BrandService(IBrandRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<List<BrandDto>> GetAllAsync()
        {
            var brands = await repo.GetAllAsync();
            return mapper.Map<List<BrandDto>>(brands);
        }

        public async Task<BrandDto?> GetByIdAsync(int id)
        {
            var entity = await repo.GetByIdAsync(id);
            return entity == null ? null : mapper.Map<BrandDto>(entity);
        }

        public async Task<BrandDto> CreateAsync(CreateBrandDto dto)
        {
            var entity = mapper.Map<Brand>(dto);
            await repo.AddAsync(entity);
            await repo.SaveChangesAsync();
            return mapper.Map<BrandDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, CreateBrandDto dto)
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