using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Domain.Interfaces;
using Bank.Bank.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Bank.Bank.Application.Services;
public class UserServices : IUserServices
{
    private readonly SignInManager<User> _signIn;
    private readonly UserManager<User> _userManager;
    public UserServices(SignInManager<User> signIn,UserManager<User> userManager)
    {
        _signIn = signIn;
        _userManager = userManager;
    }
    public async Task<UserResponse> RegisterAccount(UserRegister model)     
    {
        var identityUser = new User()
        {
            Email = model.Email,
            UserName = model.Email,
            Document = model.Document,
        };
        var result = await _userManager.CreateAsync(identityUser, model.Password);
        if (result.Succeeded)
        {
            await _userManager.SetLockoutEnabledAsync(identityUser, false);
        }
        return new UserResponse();
    }
    public async Task<UserResponse> AccountLogin(UserLogin model)
    {
        throw new NotImplementedException();
    }
}