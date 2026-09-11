using AutoMapper;
using RolexWatches.Application.DTOs.Brand;
using RolexWatches.Application.Interfaces.Repositories;
using RolexWatches.Application.Interfaces.Services;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
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
        public async Task<BrandDto> CreateAsync(CreateBrandDto dto)
        {
            var brands = await _brandRepository.GetAllAsync();
            return brands.Select(b => _mapper.Map<BrandDto>(b)).ToList();

            var entity = mapper.Map<Brand>(dto);
            await repo.AddAsync(entity);
            await repo.SaveChangesAsync();
            return mapper.Map<BrandDto>(entity);
        }

        public async Task<BrandDto?> GetByIdAsync(int id)
        public async Task<bool> DeleteAsync(int id)
        {
            var brand = await _brandRepository.GetByIdWithProductsAsync(id);
            return brand is null ? null : _mapper.Map<BrandDto>(brand);
            var entity= await repo.GetByIdAsync(id);
            if(entity== null)
            {
                return false;
            }
            repo.DeleteAsync(entity);
            return await repo.SaveChangesAsync();
        }

        public async Task<BrandDto> CreateAsync(CreateUpdateBrandDto dto)
        public async Task<List<BrandDto>> GetAllAsync()
        {
            if (await _brandRepository.NameExistsAsync(dto.Name))
                throw new InvalidOperationException($"Brand '{dto.Name}' already exists.");

            var brand = _mapper.Map<Brand>(dto);
            await _brandRepository.AddAsync(brand);
            await _brandRepository.SaveChangesAsync();

            return _mapper.Map<BrandDto>(brand);
            var brands=await repo.GetAllAsync();
            return mapper.Map<List<BrandDto>>(brands);
        }

        public async Task<BrandDto?> UpdateAsync(int id, CreateUpdateBrandDto dto)
        public async Task<bool> UpdateAsync(int id, CreateBrandDto dto)
        {
            var entity=await repo.GetByIdAsync(id);
            if(entity== null)
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
                return false;
            }
            repo.Update(entity);
            return await repo.SaveChangesAsync();

        public async Task<bool> DeleteAsync(int id)
        }
        public async Task<BrandDto?> GetByIdAsync(int id)
        {
            var brand = await _brandRepository.GetByIdWithProductsAsync(id);
            if (brand is null) return false;

            if (brand.Products.Any())
                throw new InvalidOperationException("Cannot delete a brand that still has products. Reassign or remove its products first.");

            _brandRepository.Remove(brand);
            return await _brandRepository.SaveChangesAsync();
            var entity = await repo.GetByIdAsync(id);
            return entity == null ? null : mapper.Map<BrandDto>(entity);
        }
    }
}
