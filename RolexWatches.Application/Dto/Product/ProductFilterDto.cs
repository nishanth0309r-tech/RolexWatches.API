using RolexWatches.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.DTOs.Product
{
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
}
