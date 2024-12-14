using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bank.Bank.Application.Exceptions;
using Bank.Bank.Domain.Models;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Bank.Bank.Application.Services;

public class TokenService : ITokenService
{
    private readonly IHttpContextAccessor _httpAccessor;
    public TokenService(IHttpContextAccessor httpAccessor)
    {
        _httpAccessor = httpAccessor;
    }
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
    public Guid GetUserIdFromToken()
    {
        // Fetches user ID from token stored in cookie
        var user = _httpAccessor.HttpContext!.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new AccountNotExistException("User is not authenticated");
        return Guid.Parse(userId);
    }
}