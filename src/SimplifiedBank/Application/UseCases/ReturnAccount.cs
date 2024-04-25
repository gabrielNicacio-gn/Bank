
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Repositories;
using SimplifiedBank.Infrastructure.Repositories.AccountsRepositories;

namespace SimplifiedBank.Application.UseCases;

public class ReturnAccount : IReturnAccount
{
        private readonly IAccountRepositories _account;
        public ReturnAccount(IAccountRepositories account)
        {
                _account = account;
        }

        public async Task<Account?> GetAccount(int id)
        {
                return await _account.GetAccount(id);
        }
}