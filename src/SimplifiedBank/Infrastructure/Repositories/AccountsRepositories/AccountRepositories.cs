
using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Infrastructure.Data;

namespace SimplifiedBank.Infrastructure.Repositories.AccountsRepositories;
public class AccountsRepositories : IAccountRepositories
{
    private readonly BankContext _bankContext;
    SemaphoreSlim slim = new SemaphoreSlim(1);
    public AccountsRepositories(BankContext bankContext)
    {
        _bankContext = bankContext;
    }
    public async Task<Account?> GetAccountById(int id)
    {
        slim.Wait();

        var account = await _bankContext.Accounts
        .Where(ac => ac.Id.Equals(id))
        .FirstOrDefaultAsync().ConfigureAwait(false);

        slim.Release();
        return account;
    }

    public async Task<bool> ExistAccount(string email, string password)
    {
        var account = await _bankContext.Accounts
        .AnyAsync(ac => ac.Email.Equals(email) && ac.Password.Equals(password)).ConfigureAwait(false);
        return account;
    }
}
