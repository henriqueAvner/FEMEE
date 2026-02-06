using System;
using System.Collections.Generic;
using System.Text;

namespace FEMEE.Application.DTOs.Auth
{
    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Senha { get; set; }
        
    }
}
