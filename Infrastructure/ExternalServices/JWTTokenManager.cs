using Application.Interfaces;
using Domain.Dtos.Configurations;
using FluentValidation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Shared
{
    public class JWTTokenManager : IJWTTokenManger
    {

        private readonly JWTTokenConfiguration _jwtSettings;

        public JWTTokenManager(IOptions<JWTTokenConfiguration> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        //PASS APPLICAtION USER AS PARAMETER TO GENERATE TOKEN.
        public async Task<JwtSecurityToken> GenerateJWToken()
        {
            //GET ROLES AND USERCLAIMS FROM IDENITITYSERVER USERMANAGER 

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
      
        private string RandomTokenString()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[40];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                // convert random bytes to hex string
                return BitConverter.ToString(randomBytes).Replace("-", "");
            }
        }

    }
}
