using Bank.Bank.Domain.Models;
using Bank.Bank.Infrastructure.Data;
using Bank.Bank.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bank.Bank.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly BankContext _context;

    public AccountRepository(BankContext context)
    {
        _context = context;
    }
    public async Task<Account?> GetAccountById(Guid userId)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a=>a.UserId==userId);
        return account;
    }

    public async Task<Account?> GetAccountByNumberAccount(int numberAccount)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a=>a.NumberAccount==numberAccount);
        return account;
    }

    public async Task<int> UpdateBalance(Guid userId, decimal value)
    {
        var updateRowCount =await _context.Accounts
            .Where(a=>a.UserId==userId)
            .ExecuteUpdateAsync(a=>a
                .SetProperty(account=>account.Balance, account=>account.Balance + value));
        return updateRowCount;
    }
    public async Task CreateAccount(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
    }
}