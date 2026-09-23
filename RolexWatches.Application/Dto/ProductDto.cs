using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RolexWatches.Domain.Enums;

namespace RolexWatches.Application.Dto
{
    // ---------- Read shape ----------
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public decimal EffectivePrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsLowStock { get; set; }
        public bool IsInStock { get; set; }
        public bool IsActive { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public List<ProductImageDto> Images { get; set; } = new();
        public List<ProductSpecificationDto> Specifications { get; set; } = new();
    }

    public class ProductImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductSpecificationDto
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    // ---------- Write shapes ----------
    public class CreateProductImageDto
    {
        [Required, Url]
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateProductSpecificationDto
    {
        [Required, MaxLength(100)]
        public string Key { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Value { get; set; } = string.Empty;
    }

    public class CreateProductDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Sku { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public int LowStockThreshold { get; set; } = 5;

        [Required]
        public int BrandId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<CreateProductSpecificationDto> Specifications { get; set; } = new();
        public List<CreateProductImageDto> Images { get; set; } = new();
    }

    public class UpdateProductDto : CreateProductDto
    {
        public bool IsActive { get; set; } = true;
    }

    // Used by the stock-only update endpoint (Module 3 — Inventory)
    public class UpdateStockDto
    {
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
    }

    // ---------- Search / Filter (Module 3) ----------
    public class ProductFilterDto
    {
        public string? Query { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? InStockOnly { get; set; }
        public ProductSortBy SortBy { get; set; } = ProductSortBy.Newest;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => PageSize <= 0 ? 0 : (int)System.Math.Ceiling(TotalCount / (double)PageSize);
    }
}
