using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RolexWatches.Application.DTOs.Product
{
    public class CreateProductDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Sku { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue)]
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

    public class CreateProductSpecificationDto
    {
        [Required, MaxLength(100)]
        public string Key { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Value { get; set; } = string.Empty;
    }

    public class CreateProductImageDto
    {
        [Required, Url]
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class UpdateStockDto
    {
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
    }
}