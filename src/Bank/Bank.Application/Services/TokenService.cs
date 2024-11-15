using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bank.Bank.Domain.Models;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Bank.Bank.Application.Services;

public class TokenService : ITokenService
{
    public string GenerateToken(User user)
    {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var secretKey = Encoding.ASCII.GetBytes(config.GetValue<string>("JWT:SecretKey")!);
            
        var jwtHandler = new JwtSecurityTokenHandler();
        
        var credentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),SecurityAlgorithms.HmacSha256Signature);
        var tokenDescription = new SecurityTokenDescriptor()
        {
            Subject = GenerateClaims(user),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(2),
        };
        var newToken = jwtHandler.CreateToken(tokenDescription);
        
        var tokenString = jwtHandler.WriteToken(newToken);
        return tokenString;
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name,user.Id.ToString()));
        return claims;
    }
}