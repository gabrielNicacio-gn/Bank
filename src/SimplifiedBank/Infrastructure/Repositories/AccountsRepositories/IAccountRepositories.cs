using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Infrastructure.Repositories.AccountsRepositories
{
    public interface IAccountRepositories
    {
        Task<Account?> GetAccountById(int id);
        Task<bool> ExistAccount(string email, string password);
    }
}