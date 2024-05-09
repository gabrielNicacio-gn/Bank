
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Interface;
using SimplifiedBank.Infrastructure.Repositories.AccountsRepositories;
using SimplifiedBank.Interfaces.Exceptions;

namespace SimplifiedBank.Application.UseCases;

public class ReturnAccount : IReturnAccount
{
        private readonly IAccountRepositories _account;
        public ReturnAccount(IAccountRepositories account)
        {
                _account = account;
        }

        public async Task<Account> GetAccount(int id)
        {
                var account = await _account.GetAccount(id).ConfigureAwait(false)
                ?? throw new UserNotFoundException("Conta(s) não encontrada(s)");

                return account;
        }
}