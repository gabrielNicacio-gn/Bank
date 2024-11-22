using System.Security.Claims;
using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Application.Exceptions;
using Bank.Bank.Domain.Interfaces;
using Bank.Bank.Domain.Models;
using Bank.Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bank.Bank.Application.Services;
public class AccountServices : IAccountServices
{
    private readonly BankContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AccountServices(BankContext context,IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }
    public async Task<AccountResponse> GetAccount()
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new AccountNotExistException("User is not authenticated");
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a=>a.UserId==Guid.Parse(userId))
            ?? new Account();
       return new AccountResponse()
            { Balance = account.Balance, IdAccount = account.AccountId, NumberAccount = account.NumberAccount };
    }
    public async Task<AccountResponse> AddBalance(AddBalanceDto addBalance)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new AccountNotExistException("User is not authenticated");
            var count=await _context.Accounts
                .Where(a=>a.UserId==Guid.Parse(userId))
                .ExecuteUpdateAsync(a=>a
                    .SetProperty(account=>account.Balance, account=>account.Balance + addBalance.Value));
            if (count > 0)
            {
                return new AccountResponse() { Balance = addBalance.Value, };
            }
            throw new NpgsqlException();
    }
}