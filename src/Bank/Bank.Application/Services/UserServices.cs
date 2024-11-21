using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Domain.Interfaces;
using Bank.Bank.Domain.Models;
using Bank.Bank.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Bank.Bank.Application.Services;
public class UserServices : IUserServices   
{
    private readonly SignInManager<User> _signIn;
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly BankContext _context;
    public UserServices(SignInManager<User> signIn,UserManager<User> userManager
        ,ITokenService tokenService,BankContext context)
    {
        _context = context;
        _signIn = signIn;
        _userManager = userManager;
        _tokenService = tokenService;
    }
    public async Task<DefaultResponse<UserResponse>> RegisterAccount(UserRegisterDto model)     
    {
        var identityUser = new User()
        {
            Email = model.Email,
            UserName = model.Email,
            Document = model.Document,
            EmailConfirmed = true,
        };
        var passwordHasher = new PasswordHasher<User>();
        var hash = passwordHasher.HashPassword(identityUser, model.Password);
        identityUser.PasswordHash = hash;
        Console.WriteLine($"{_userManager}");
        var result = await _userManager.CreateAsync(identityUser);
        if (result.Succeeded)
        {
            var account = new Account() {UserId = identityUser.Id };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            await _userManager.SetLockoutEnabledAsync(identityUser, false);
            var data = new UserResponse() {IdUser = identityUser.Id};
            return new DefaultResponse<UserResponse>(data);
        }
        return new DefaultResponse<UserResponse>("Failed to create account");
    }
    public async Task<DefaultResponse<UserResponse>> AccountLogin(UserLoginDto model)
    {
        var userExist = await _userManager.FindByEmailAsync(model.Email) ?? throw new Exception();
        if(await _userManager.CheckPasswordAsync(userExist,model.Password))
        {
           var token= _tokenService.GenerateToken(userExist);
           await _signIn.SignInAsync(userExist,false,token);
           return new DefaultResponse<UserResponse>(new UserResponse());
        }
        return new DefaultResponse<UserResponse>("Fail to login");
    }

    public async Task<DefaultResponse<UserResponse>> Logout()
    {
        await _signIn.SignOutAsync();
        return new DefaultResponse<UserResponse>(new UserResponse());
    }
}