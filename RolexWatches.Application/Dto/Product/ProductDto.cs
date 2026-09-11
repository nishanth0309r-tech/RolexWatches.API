using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.DTOs.Product
{
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

        public DateTime CreatedAt { get; set; }
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
}
