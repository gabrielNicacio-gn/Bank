
using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Domain.Models;

namespace Bank.Bank.Domain.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
    Guid GetUserIdFromToken();
}