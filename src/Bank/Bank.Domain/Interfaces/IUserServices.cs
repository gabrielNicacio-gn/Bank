using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;

namespace Bank.Bank.Domain.Interfaces;

public interface IUserServices
{
    Task<DefaultResponse<UserResponse>> RegisterAccount(UserRegisterDto model);
    Task<DefaultResponse<UserResponse>> AccountLogin(UserLoginDto model);
    Task<DefaultResponse<UserResponse>> Logout();
}