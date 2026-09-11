using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.Dto
{
    public class WishlistItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class AddToWishlistDto
    {
        public int ProductId { get; set; }
    }
}