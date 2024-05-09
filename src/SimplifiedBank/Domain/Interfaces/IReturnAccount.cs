
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Domain.Interface;

public interface IReturnAccount
{
        Task<Account> GetAccount(int id);
}