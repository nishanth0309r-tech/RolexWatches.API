using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RolexWatches.Application.DTOs.Category;
using RolexWatches.Application.Interfaces.Repositories;
using RolexWatches.Application.Interfaces.Services;
using RolexWatches.Domain.Entities;

namespace RolexWatches.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetTopLevelWithChildrenAsync();
            return categories.Select(c => _mapper.Map<CategoryDto>(c)).ToList();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category is null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto dto)
        {
            if (await _categoryRepository.NameExistsAsync(dto.Name))
                throw new InvalidOperationException($"Category '{dto.Name}' already exists.");

            var category = _mapper.Map<Category>(dto);
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto?> UpdateAsync(int id, CreateUpdateCategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null) return null;

            if (dto.ParentCategoryId == id)
                throw new InvalidOperationException("A category cannot be its own parent.");

            if (await _categoryRepository.NameExistsAsync(dto.Name, excludeCategoryId: id))
                throw new InvalidOperationException($"Category '{dto.Name}' already exists.");

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.ParentCategoryId = dto.ParentCategoryId;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null) return false;

            if (await _categoryRepository.HasProductsAsync(id))
                throw new InvalidOperationException("Cannot delete a category that still has products.");

            _categoryRepository.Remove(category);
            return await _categoryRepository.SaveChangesAsync();
        }
    }
}