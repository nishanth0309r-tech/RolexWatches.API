using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolexWatches.Application.Dto
{
    public class PayRequestDto
    {
        public int OrderId { get; set; }
        public string Method { get; set; } = "Card";
        public string? CardNumber { get; set; }
    }
}
