using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Domain.Models;

namespace Bank.Bank.Domain.Interfaces;

public interface IAccountServices
{
    Task<AccountResponse> GetAccount(Guid idUser);
    Task<AccountResponse> AddBalance(AddBalanceDto addBalance,Guid userId);
}