using Domain.Models.Responses;
using MediatR;

namespace Domain.Models.Requests
{
    public class AuthanticationRequest : IRequest<AuthenticationResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
