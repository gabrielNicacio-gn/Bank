using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using SimplifiedBank.Application.DTOs.Login;
using SimplifiedBank.Infrastructure.Repositories.AccountsRepositories;

namespace SimplifiedBank.Application.Authentication
{
    public class TokenService
    {
        private readonly IAccountRepositories _accountRepository;
        public TokenService(IAccountRepositories accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public string GenerateToken(LoginDTO login, IConfiguration _config)
        {
            var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
                throw new InvalidOperationException("Invalid Key");

            var privateKey = Encoding.UTF8.GetBytes(key);

            var singnigCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
             SecurityAlgorithms.HmacSha256Signature);

            var generateClaims = new List<Claim> { new Claim(type: ClaimTypes.Name , login.Email),
             new Claim(type: ClaimTypes.Name , login.Password)};

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(generateClaims),
                Audience = _config.GetSection("JWT").GetValue<string>("Audience"),
                Expires = DateTime.UtcNow.AddHours(4),
                Issuer = _config.GetSection("JWT").GetValue<string>("Issuer"),
                SigningCredentials = singnigCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var newToken = tokenHandler.WriteToken(token);
            return newToken;
        }
    }
}