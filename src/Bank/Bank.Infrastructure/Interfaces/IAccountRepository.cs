using Bank.Bank.Domain.Models;

namespace Bank.Bank.Infrastructure.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetAccountById(Guid userId);
    Task<Account?> GetAccountByNumberAccount(int numberAccount);
    Task<int> UpdateBalance(Guid userId, decimal value);
    Task CreateAccount(Account account);

}