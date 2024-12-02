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
        var secretSigningKey = Encoding.ASCII.GetBytes
            (ConfigurationAppSettingsJson().GetValue<string>("JWT:SecretKey")!);
            
        var jwtHandler = new JwtSecurityTokenHandler();
        
        var credentials = new SigningCredentials(new SymmetricSecurityKey(secretSigningKey)
            ,SecurityAlgorithms.HmacSha256Signature);
        var tokenDescription = new SecurityTokenDescriptor()
        {
            Subject = GenerateClaims(user),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = ConfigurationAppSettingsJson().GetValue<string>("JWT:Issuer") ,
            IssuedAt = DateTime.UtcNow
        };
        var newToken = jwtHandler.CreateToken(tokenDescription);
        
        var encodedToken = jwtHandler.WriteToken(newToken);
        return encodedToken;
    }
    
    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name,user.Id.ToString()));
        return claims;
    }
    private static IConfigurationRoot ConfigurationAppSettingsJson()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();
        return configuration;
    }
}