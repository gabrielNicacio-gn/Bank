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
using SimplifiedBank.Interfaces.Exceptions;

namespace SimplifiedBank.Application.Authentication
{
    public class TokenService
    {
        private readonly IAccountRepositories _accountRepository;
        public TokenService(IAccountRepositories accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<string?> GenerateToken(LoginDTO login, IConfiguration _config)
        {
            var account = await _accountRepository.ExistAccount(login.Email, login.Password);
            if (!account)
                throw new UserNotFoundException("Não Existe");

            var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
                throw new InvalidOperationException("Invalid Key");

            var privateKey = Encoding.UTF8.GetBytes(key);

            var singnigCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
             SecurityAlgorithms.HmacSha256Signature);

            var generateClaims = new[] { new Claim(type: ClaimTypes.Name, login.Email) };

            var tokenDescriptor = new JwtSecurityToken(
                audience: _config.GetSection("JWT").GetValue<string>("Audience"),
                claims: generateClaims,
                issuer: _config.GetSection("JWT").GetValue<string>("Issuer"),
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: singnigCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var newToken = tokenHandler.WriteToken(tokenDescriptor);
            return newToken;
        }
    }
}