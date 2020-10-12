using System.IdentityModel.Tokens.Jwt;

namespace Application.Interfaces
{
    public interface IJWTTokenManger
    {
        JwtSecurityToken GenerateJWToken();
    }
}