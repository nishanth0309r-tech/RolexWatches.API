using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;
using RolexWatches.Domain.Entities;
using RolexWatches.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
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

            entity.Specifications = dto.Specifications
                .Select(s => new ProductSpecification { Key = s.Key, Value = s.Value })
                .ToList();

            entity.Images = dto.Images
                .Select(i => new ProductImage { ImageUrl = i.ImageUrl, IsPrimary = i.IsPrimary, DisplayOrder = i.DisplayOrder })
                .ToList();

            await repo.AddAsync(entity);
            await repo.SaveChangesAsync();
            return mapper.Map<ProductDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) return false;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Sku = dto.Sku;
            entity.Price = dto.Price;
            entity.DiscountPrice = dto.DiscountPrice;
            entity.StockQuantity = dto.StockQuantity;
            entity.LowStockThreshold = dto.LowStockThreshold;
            entity.BrandId = dto.BrandId;
            entity.CategoryId = dto.CategoryId;
            entity.IsActive = dto.IsActive;

            entity.Specifications.Clear();
            foreach (var s in dto.Specifications)
                entity.Specifications.Add(new ProductSpecification { Key = s.Key, Value = s.Value });

            entity.Images.Clear();
            foreach (var i in dto.Images)
                entity.Images.Add(new ProductImage { ImageUrl = i.ImageUrl, IsPrimary = i.IsPrimary, DisplayOrder = i.DisplayOrder });

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

        // ---------- Module 3: Search / Filter / Inventory ----------

        public async Task<PagedResult<ProductDto>> SearchAsync(ProductFilterDto filter)
        {
            var (items, totalCount) = await repo.SearchAsync(
                filter.Query, filter.BrandId, filter.CategoryId,
                filter.MinPrice, filter.MaxPrice, filter.InStockOnly,
                filter.SortBy, filter.PageNumber, filter.PageSize);

            return new PagedResult<ProductDto>
            {
                Items = mapper.Map<List<ProductDto>>(items),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<bool> UpdateStockAsync(int id, int newStockQuantity)
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) return false;

            entity.StockQuantity = newStockQuantity;
            repo.Update(entity);
            return await repo.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetLowStockAsync()
        {
            var lowStock = await repo.GetLowStockAsync();
            return mapper.Map<List<ProductDto>>(lowStock);
        }
    }
}