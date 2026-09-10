using RolexWatches.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.ServiceInterface
{
    public interface IAuthService
    {
        
            Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
            Task<AuthResponseDto> LoginAsync(LoginDto dto);
        
    }
}
