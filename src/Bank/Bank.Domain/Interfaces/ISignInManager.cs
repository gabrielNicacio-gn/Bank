using Bank.Bank.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Bank.Bank.Domain.Interfaces;

public interface ISignInManager : SignInManager<User>
{
    
}