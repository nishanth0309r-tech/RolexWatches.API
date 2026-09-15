using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RolexWatches.Application.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository repo;
        private readonly IMapper mapper;

        public CategoryService(ICategoryRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await repo.GetAllAsync();
            return mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var entity = await repo.GetByIdAsync(id);
            return entity == null ? null : mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var entity = mapper.Map<Category>(dto);
            await repo.AddAsync(entity);
            await repo.SaveChangesAsync();
            return mapper.Map<CategoryDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, CreateCategoryDto dto)
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