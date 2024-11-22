using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;

namespace Bank.Bank.Domain.Interfaces;

public interface IUserServices
{
    Task<UserResponse> RegisterUserAndAccount(UserRegisterDto model);
    Task<UserResponse> AccountLogin(UserLoginDto model);
    Task<UserResponse> Logout();
}