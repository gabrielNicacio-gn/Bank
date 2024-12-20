using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Application.Exceptions;
using Bank.Bank.Domain.Interfaces;
using Bank.Bank.Domain.Models;
using Bank.Bank.Infrastructure.Data;
using Bank.Bank.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Bank.Bank.Application.Services;
    public class UserServices : IUserServices   
    {
        private readonly SignInManager<User> _signIn;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IAccountRepository _accountRepository;
        public UserServices(SignInManager<User> signIn,UserManager<User> userManager    
            ,ITokenService tokenService,IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _signIn = signIn;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<UserResponse> RegisterUserAndAccount(UserRegisterDto model)     
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
            
            var result = await _userManager.CreateAsync(identityUser);
            if (!result.Succeeded) 
            { 
                var errors = result.Errors.Select(e => e.Code == "DuplicateUserName"); 
                throw new DuplicateUserNameException("Duplicate UserName");
            }
            
            var account = new Account() {UserId = identityUser.Id };
            await _accountRepository.CreateAccount(account);
            
            await _userManager.SetLockoutEnabledAsync(identityUser, false);
            return new UserResponse() {IdUser = identityUser.Id};
        }
        public async Task<UserResponse> AccountLogin(UserLoginDto model)
        {
            var userExist = await _userManager.FindByEmailAsync(model.Email) 
                            ?? throw new IncorrectEmailOrPasswordException("Email or password is incorrect");
            if(await _userManager.CheckPasswordAsync(userExist,model.Password))
            {
                var token= _tokenService.GenerateToken(userExist);
                await _signIn.SignInAsync(userExist,false,token);
                return new UserResponse();
            }
            throw new IncorrectEmailOrPasswordException("Email or password is incorrect");
        }

        public async Task<UserResponse> Logout()
        {
            await _signIn.SignOutAsync();
            return new UserResponse();
        }
}