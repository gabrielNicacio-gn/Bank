using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Domain.Models;

namespace Bank.Bank.Domain.Interfaces;

public interface IAccountServices
{
    Task<DefaultResponse<AccountResponse>> GetAccount();
    Task<DefaultResponse<AccountResponse>> AddBalance(AddBalanceDto addBalance);
}