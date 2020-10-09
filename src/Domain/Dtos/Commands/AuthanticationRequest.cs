using Domain.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Requests
{
    public class AuthanticationRequest : IRequest<AuthenticationResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
