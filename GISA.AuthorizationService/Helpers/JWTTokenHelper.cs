using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GISA.AuthorizationService.Helpers
{
    public static class JWTTokenHelper
    {
        public static string GenerateToken(string symetricKey, 
        double tokenDuration, 
        string usuarioLogin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("PessoaId", usuarioLogin.Split('-')[1]),
                    new Claim("PessoaTipo", usuarioLogin.Split('-')[0])
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenDuration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(symetricKey)),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}