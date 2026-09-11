using AutoMapper;
using RolexWatches.Application.DTOs.Brand;
using RolexWatches.Application.Interfaces.Repositories;
using RolexWatches.Application.Interfaces.Services;
using RolexWatches.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.Service
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<List<BrandDto>> GetAllAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            return brands.Select(b => _mapper.Map<BrandDto>(b)).ToList();
        }

        public async Task<BrandDto?> GetByIdAsync(int id)
        {
            var brand = await _brandRepository.GetByIdWithProductsAsync(id);
            return brand is null ? null : _mapper.Map<BrandDto>(brand);
        }

        public async Task<BrandDto> CreateAsync(CreateUpdateBrandDto dto)
        {
            if (await _brandRepository.NameExistsAsync(dto.Name))
                throw new InvalidOperationException($"Brand '{dto.Name}' already exists.");

            var brand = _mapper.Map<Brand>(dto);
            await _brandRepository.AddAsync(brand);
            await _brandRepository.SaveChangesAsync();

            return _mapper.Map<BrandDto>(brand);
        }

        public async Task<BrandDto?> UpdateAsync(int id, CreateUpdateBrandDto dto)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand is null) return null;

            if (await _brandRepository.NameExistsAsync(dto.Name, excludeBrandId: id))
                throw new InvalidOperationException($"Brand '{dto.Name}' already exists.");

            brand.Name = dto.Name;
            brand.LogoUrl = dto.LogoUrl;
            brand.Description = dto.Description;
            brand.IsActive = dto.IsActive;

            _brandRepository.Update(brand);
            await _brandRepository.SaveChangesAsync();

            return _mapper.Map<BrandDto>(brand);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var brand = await _brandRepository.GetByIdWithProductsAsync(id);
            if (brand is null) return false;

            if (brand.Products.Any())
                throw new InvalidOperationException("Cannot delete a brand that still has products. Reassign or remove its products first.");

            _brandRepository.Remove(brand);
            return await _brandRepository.SaveChangesAsync();
        }
    }
}
