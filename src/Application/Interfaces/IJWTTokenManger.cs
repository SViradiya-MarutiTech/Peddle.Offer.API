using Domain.Dtos.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IJWTTokenManger
    {
        Task<JwtSecurityToken> GenerateJWToken();
    }
}