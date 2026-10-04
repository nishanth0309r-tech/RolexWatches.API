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
            // 1. The parent, if given, must exist.
            if (dto.ParentCategoryId.HasValue &&
                await repo.GetByIdAsync(dto.ParentCategoryId.Value) == null)
                throw new InvalidOperationException("Parent category not found.");

            var entity = mapper.Map<Category>(dto);
            entity.Name = entity.Name.Trim();

            await repo.AddAsync(entity);
            if (!await repo.SaveChangesAsync())
                throw new InvalidOperationException("Failed to save category.");

            // 2. Re-fetch so the returned DTO has ParentCategoryName and ProductCount filled in.
            var saved = await repo.GetByIdAsync(entity.Id);
            return mapper.Map<CategoryDto>(saved);
        }

        public async Task<bool> UpdateAsync(int id, CreateCategoryDto dto)
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) return false;

            if (dto.ParentCategoryId.HasValue)
            {
                if (await repo.GetByIdAsync(dto.ParentCategoryId.Value) == null)
                    throw new InvalidOperationException("Parent category not found.");

                // Walk up from the proposed parent. If we reach this category, we'd create a loop.
                var parentId = dto.ParentCategoryId;
                while (parentId.HasValue)
                {
                    if (parentId.Value == id)
                        throw new InvalidOperationException(
                            "A category cannot be placed under itself or one of its own subcategories.");
                    var parent = await repo.GetByIdAsync(parentId.Value);
                    parentId = parent?.ParentCategoryId;
                }
            }

            mapper.Map(dto, entity);
            await repo.SaveChangesAsync();   
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) return false;

            if (entity.Products.Count > 0)
                throw new InvalidOperationException("This category still has products.");

            var all = await repo.GetAllAsync();
            if (all.Any(c => c.ParentCategoryId == id))
                throw new InvalidOperationException("This category still has subcategories.");

            repo.Delete(entity);
            await repo.SaveChangesAsync();
            return true;
        }
    }
}