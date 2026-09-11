using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.Dto
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserDto User { get; set; } = new();
    }
}
