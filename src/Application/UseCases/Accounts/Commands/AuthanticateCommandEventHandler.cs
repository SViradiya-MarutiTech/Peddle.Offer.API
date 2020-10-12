using Application.Interfaces;
using Domain.Models.Requests;
using Domain.Models.Responses;
using MediatR;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Accounts.Commands
{
    public class AuthanticateCommandEventHandler : IRequestHandler<AuthanticationRequest, AuthenticationResponseDto>
    {
        private readonly IJWTTokenManger _jwtManager;

        public AuthanticateCommandEventHandler(IJWTTokenManger jwtManager)
        {
            _jwtManager = jwtManager;
        }
        public async Task<AuthenticationResponseDto> Handle(AuthanticationRequest request, CancellationToken cancellationToken)
        {
            //VALIDATE USER AGAINST DATABASE HERE.

            //FOLLOWING CODE WILL GENERATE JWT TOKEN FOR VALID USER.
            JwtSecurityToken jwtSecurityToken = _jwtManager.GenerateJWToken();
            AuthenticationResponseDto response = new AuthenticationResponseDto();
            response.Id = 34542;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = request.Email;
            response.UserName = "seller name";
            response.Roles = new List<string>() { "seller" };
            response.IsVerified = true;
           
            return response;
        }
    }
}
