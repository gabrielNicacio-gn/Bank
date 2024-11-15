using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;

namespace Bank.Bank.Domain.Interfaces;

public interface IUserServices
{
    Task<UserResponse> RegisterAccount(UserRegister model);
    Task<UserResponse> AccountLogin(UserLogin model);
}