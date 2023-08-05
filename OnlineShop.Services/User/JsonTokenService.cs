using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Shared.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.User
{
    public class JsonTokenService : IJsonTokenService
    {
        //private JsonTokenOptions _tokenOptions;

        //public JsonTokenService(IOptions<JsonTokenOptions> tokenOptions)
        //{
        //    _tokenOptions = tokenOptions.Value;
        //}
        public string GenerateToken(Models.User user, List<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JsonTokenOptions.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                JsonTokenOptions.Issuer,
                JsonTokenOptions.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
