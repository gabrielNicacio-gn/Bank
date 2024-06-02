
using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Infrastructure.Data;

namespace SimplifiedBank.Infrastructure.Repositories.AccountsRepositories;
public class AccountsRepositories : IAccountRepositories
{
    private readonly BankContext _bankContext;
    public AccountsRepositories(BankContext bankContext)
    {
        _bankContext = bankContext;
    }
    public Account? GetAccountById(int id)
    {
        var account = _bankContext.Accounts
        .Where(ac => ac.Id.Equals(id))
        .FirstOrDefault();
        return account;
    }

    public bool ExistAccount(string email, string password)
    {
        var account = _bankContext.Accounts
        .Any(ac => ac.Email.Equals(email) && ac.Password.Equals(password));
        return account;
    }
}
