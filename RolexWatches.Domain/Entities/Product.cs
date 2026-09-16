using System;
using System.Collections.Generic;

namespace RolexWatches.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Sku { get; set; } = string.Empty;

        

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public int StockQuantity { get; set; }

        public int LowStockThreshold { get; set; } = 5;

        public bool IsActive { get; set; } = true;

        // Brand
        public int BrandId { get; set; }

        public Brand? Brand { get; set; }

        // Category
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        // Navigation properties
        public ICollection<ProductImage> Images { get; set; }
            = [];

        public ICollection<ProductSpecification> Specifications { get; set; }
            = [];

        public ICollection<Review> Reviews { get; set; }
            = [];

        public ICollection<OrderItem> OrderItems { get; set; }
            = [];

        // Dates
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Computed properties
        public bool IsLowStock =>
            StockQuantity <= LowStockThreshold;

        public bool IsInStock =>
            StockQuantity > 0;

        public decimal EffectivePrice =>
            DiscountPrice.HasValue && DiscountPrice.Value < Price
                ? DiscountPrice.Value
                : Price;
    
        
    }
}