using Bank.Bank.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Bank.Bank.Domain.Interfaces;

public interface IUserManager
{
    Task<IdentityResult> CreateAsync(User user);
    Task<IdentityResult> SetLockoutEnabledAsync(User user, bool enabled);
    Task<bool> CheckPasswordAsync(User user, string password);
}