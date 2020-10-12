using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Dtos.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.ExternalServices
{
    public class JWTTokenManager : IJWTTokenManger
    {

        private readonly JWTTokenConfiguration _jwtSettings;

        public JWTTokenManager(IOptions<JWTTokenConfiguration> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        //pass application user as parameter to generate token.
        public JwtSecurityToken GenerateJWToken()
        {
            //get roles and userclaims from idenitityserver usermanager 

            var roleClaims = new List<Claim>();
            roleClaims.Add(new Claim("roles", "seller"));
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "Seller Name"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, "selleruser@gmail.com"),
                new Claim("uid", "45231")
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
