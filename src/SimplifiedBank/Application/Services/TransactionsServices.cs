
using Repositories = SimplifiedBank.Infrastructure.Repositories;
using DataDTOs = SimplifiedBank.Application.DTOs;
using SimplifiedBank.Interfaces.Exceptions;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.Services;
public class TransactionsServices : ITransactionsServices
{
    private readonly Repositories::AccountsRepositories.IAccountRepositories _accountRepository;
    public TransactionsServices(Repositories::AccountsRepositories.IAccountRepositories accountRepository)
    {
        _accountRepository = accountRepository;
    }
    public void ValidateTransaction(DataDTOs::Request.TransactionCreationData data)
    {
        var account = CheckAccountsExistence(data.IdSender);
        CheckAccountsExistence(data.IdReceiver);
        CheckIfTheBalanceIsSufficient(account, data.Value);
    }
    private void CheckIfTheBalanceIsSufficient(Account account, decimal Value)
    {
        if (account.Balance < Value)
            throw new InsufficienteBalanceException("Saldo Insuficiente");
    }
    private Account CheckAccountsExistence(int accountId)
    {
        var taskSender = _accountRepository.GetAccountById(accountId)
        ?? throw new UserNotFoundException("Conta(s) não encontrada(s)");

        return taskSender;
    }
}

