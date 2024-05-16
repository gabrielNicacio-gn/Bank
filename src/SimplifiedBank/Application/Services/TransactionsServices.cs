
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
        var taskSender = await _accountRepository.GetAccountById(data.IdSender);
        var taskReceiver = await _accountRepository.GetAccountById(data.IdReceiver);

        if (taskSender is null || taskReceiver is null)
            throw new UserNotFoundException("Contas não encontradas");

        return [taskSender, taskReceiver];
    }
}

