
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Domain.Repositories;

public interface IReturnAccount
{
        Task<Account> GetAccount(int id);
}