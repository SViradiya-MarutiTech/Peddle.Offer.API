using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dtos.Commands
{
    public class AuthenticationRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
