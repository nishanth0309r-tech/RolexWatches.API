using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Domain.Enums
{
    public enum StockStatus
    {
        OutOfStock = 0,
        LowStock = 1,
        InStock = 2
    }
    public enum ProductSortBy
    {
        Newest = 0,
        PriceLowToHigh = 1,
        PriceHighToLow = 2,
        NameAToZ = 3,
        NameZToA = 4
    }
}