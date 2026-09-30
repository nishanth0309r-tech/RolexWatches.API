using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolexWatches.Application.Dto
{
    public class CreateOrderDto
    {
        public string ShippingAddress { get; set; } = string.Empty;
        public List<OrderItemRequestDto> Items { get; set; } = new();
    }
}
