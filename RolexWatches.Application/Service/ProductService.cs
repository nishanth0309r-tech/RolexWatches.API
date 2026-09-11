using AutoMapper;
using RolexWatches.Application.DTOs.Common;
using RolexWatches.Application.DTOs.Product;
using RolexWatches.Application.Interfaces.Repositories;
using RolexWatches.Application.Interfaces.Services;
using RolexWatches.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdWithDetailsAsync(id);
            return product is null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<PagedResult<ProductDto>> SearchAsync(ProductFilterDto filter)
        {
            var (items, totalCount) = await _productRepository.SearchAsync(filter);

            return new PagedResult<ProductDto>
            {
                Items = items.Select(p => _mapper.Map<ProductDto>(p)).ToList(),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            if (await _productRepository.SkuExistsAsync(dto.Sku))
                throw new InvalidOperationException($"A product with SKU '{dto.Sku}' already exists.");

            var product = _mapper.Map<Product>(dto);

            foreach (var spec in dto.Specifications)
                product.Specifications.Add(_mapper.Map<ProductSpecification>(spec));

            foreach (var image in dto.Images)
                product.Images.Add(_mapper.Map<ProductImage>(image));

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            var created = await _productRepository.GetByIdWithDetailsAsync(product.Id);
            return _mapper.Map<ProductDto>(created);
        }

        public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdWithDetailsAsync(id);
            if (product is null) return null;

            if (await _productRepository.SkuExistsAsync(dto.Sku, excludeProductId: id))
                throw new InvalidOperationException($"A product with SKU '{dto.Sku}' already exists.");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Sku = dto.Sku;
            product.Price = dto.Price;
            product.DiscountPrice = dto.DiscountPrice;
            product.StockQuantity = dto.StockQuantity;
            product.LowStockThreshold = dto.LowStockThreshold;
            product.BrandId = dto.BrandId;
            product.CategoryId = dto.CategoryId;
            product.IsActive = dto.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            product.Specifications.Clear();
            foreach (var spec in dto.Specifications)
                product.Specifications.Add(_mapper.Map<ProductSpecification>(spec));

            product.Images.Clear();
            foreach (var image in dto.Images)
                product.Images.Add(_mapper.Map<ProductImage>(image));

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            var updated = await _productRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ProductDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return false;

            _productRepository.Remove(product);
            return await _productRepository.SaveChangesAsync();
        }

        // ----- Module 3: Discovery / Inventory -----

        public async Task<ProductDto?> UpdateStockAsync(int id, int newStockQuantity)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return null;

            product.StockQuantity = newStockQuantity;
            product.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            var updated = await _productRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ProductDto>(updated);
        }

        public async Task<PagedResult<ProductDto>> GetLowStockAsync(int pageNumber, int pageSize)
        {
            var lowStock = await _productRepository.GetLowStockProductsAsync();

            var page = lowStock
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => _mapper.Map<ProductDto>(p))
                .ToList();

            return new PagedResult<ProductDto>
            {
                Items = page,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = lowStock.Count
            };
        }
    }
}
