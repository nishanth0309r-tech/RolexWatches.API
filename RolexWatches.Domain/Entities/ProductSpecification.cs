using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Domain.Entities
{
    public class ProductSpecification
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string Key { get; set; } = string.Empty;   // e.g. "Case Material"
        public string Value { get; set; } = string.Empty; // e.g. "Stainless Steel"
    }
}
