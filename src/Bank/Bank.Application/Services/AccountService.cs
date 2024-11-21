using System.Security.Claims;
using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Domain.Interfaces;
using Bank.Bank.Domain.Models;
using Bank.Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
    public async Task<DefaultResponse<AccountResponse>> GetAccount()
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new Exception();
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a=>a.UserId==Guid.Parse(userId))
            ?? new Account();
        var data = new AccountResponse()
            { Balance = account.Balance, IdAccount = account.AccountId, NumberAccount = account.NumberAccount };
        return new DefaultResponse<AccountResponse>(data);
    }
    public async Task<DefaultResponse<AccountResponse>> AddBalance(AddBalanceDto addBalance)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new Exception();
        using (var transaction = _context.Database.BeginTransaction())
        {
            var count=await _context.Accounts
                .Where(a=>a.UserId==Guid.Parse(userId))
                .ExecuteUpdateAsync(a=>a
                    .SetProperty(account=>account.Balance, account=>account.Balance + addBalance.Value));
            if (count > 0)
            {
                await transaction.CommitAsync();
                var account = new AccountResponse() { Balance = addBalance.Value, };
                return new DefaultResponse<AccountResponse>(account,"Value deposited with success");
            }
            await transaction.RollbackAsync();
            return new DefaultResponse<AccountResponse>("Fail to deposited");
        }
    }
}