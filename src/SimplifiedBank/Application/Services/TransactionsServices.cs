
using Repositories = SimplifiedBank.Infrastructure.Repositories;
using DataDTOs = SimplifiedBank.Application.DTOs;
using SimplifiedBank.Interfaces.Exceptions;
using SimplifiedBank.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace SimplifiedBank.Application.Services;
public class TransactionsServices : ITransactionsServices
{
    private readonly Repositories::AccountsRepositories.IAccountRepositories _accountRepository;
    public TransactionsServices(Repositories::AccountsRepositories.IAccountRepositories accountRepository)
    {
        _accountRepository = accountRepository;
    }
    public async Task ValidateTransaction(DataDTOs::Request.TransactionCreationData data)
    {
        var account = await CheckAccountsExistence(data).ConfigureAwait(false);
        var sender = account.First(ac => ac.Id == data.IdSender);
        CheckIfTheBalanceIsSufficient(sender, data.Value);
    }
    private void CheckIfTheBalanceIsSufficient(Account account, decimal Value)
    {
        if (account.Balance < Value)
            throw new InsufficienteBalanceException("Saldo Insuficiente");
    }
    private async Task<Account[]> CheckAccountsExistence(DataDTOs::Request.TransactionCreationData data)
    {
        var taskSender = _accountRepository.GetAccountById(data.IdSender);
        var taskReceiver = _accountRepository.GetAccountById(data.IdReceiver);

        Account[] accounts = new Account[2];

        var currentAccount = await Task.WhenAny([taskSender, taskReceiver]).ConfigureAwait(false)
        ?? throw new UserNotFoundException("Conta(s) não encontrada(ss)");

        if (currentAccount == taskSender)
        {
            var sender = await taskSender.ConfigureAwait(false);
            accounts[0] = sender!;
        }
        else
        {
            var receiver = await taskReceiver.ConfigureAwait(false);
            accounts[1] = receiver!;
        }

        return accounts;
    }
}

