using System.Security.Claims;
using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Application.Exceptions;
using Bank.Bank.Domain.Interfaces;
using Bank.Bank.Domain.Models;
using Bank.Bank.Infrastructure.Data;
using Bank.Bank.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bank.Bank.Application.Services;
public class AccountServices : IAccountServices
{
    private readonly IAccountRepository _accountRepository;

    public AccountServices(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    public async Task<AccountResponse> GetAccount(Guid userId)
    { 
        var account = await _accountRepository.GetAccountById(userId) 
                      ?? throw new AccountNotExistException("Account not found");
       return new AccountResponse()
            { Balance = account.Balance, IdAccount = account.AccountId, NumberAccount = account.NumberAccount };
    }
    public async Task<AccountResponse> AddBalance(AddBalanceDto addBalance,Guid userId)
    {
        var updateRowCount = await _accountRepository.UpdateBalance(userId,addBalance.Value);
        if (updateRowCount > 0)
        {
            return new AccountResponse() { Balance = addBalance.Value, };
        }
        throw new FailUpdateBalanceException("Failed to update balance");
    }
}