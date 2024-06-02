
using SimplifiedBank.Application.DTOs.Response;
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

        public GetAccountData GetAccount(int id)
        {
                var account = _account.GetAccountById(id)
                ?? throw new UserNotFoundException("Conta(s) não encontrada(s)");

                var viewAccount = new GetAccountData(account.Id, account.FullName, account.Balance);

                return viewAccount;
        }
}