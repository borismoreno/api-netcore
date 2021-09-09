using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiNetCore.Helpers
{
    public class jwt
    {
        private readonly IConfiguration configuration;
        public jwt(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GenerarJWT(string uid, string nombre)
        {
            var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("secretkey"));
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, nombre));
            claims.AddClaim(new Claim("uid", uid));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.Now.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(createdToken);
            return token;
        }
    }
}