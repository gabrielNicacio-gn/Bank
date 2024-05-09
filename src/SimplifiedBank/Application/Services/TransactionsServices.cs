using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories = SimplifiedBank.Infrastructure.Repositories;
using DataDTOs = SimplifiedBank.Application.DTOs;
using SimplifiedBank.Interfaces.Exceptions;
using SimplifiedBank.Domain.Entities;
using Microsoft.VisualBasic;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.OpenApi.Validations;

namespace SimplifiedBank.Application.Services;
public class TransactionsServices
{
    private readonly Repositories::AccountsRepositories.IAccountRepositories _accountRepository;
    public TransactionsServices(Repositories::AccountsRepositories.IAccountRepositories accountRepository)
    {
        _accountRepository = accountRepository;
    }
    public async Task Validation(DataDTOs::Request.TransactioCreationData data, decimal value)
    {
        await ExistAccounts(data, value).ConfigureAwait(false);
    }
    private void IsBalanceSufficient(Account account, decimal Value)
    {
        if (account.Balance < Value)
            throw new InsufficienteBalanceException("Saldo Insuficiente");
    }
    private async Task ExistAccounts(DataDTOs::Request.TransactioCreationData data, decimal value)
    {
        var sender = await _accountRepository.GetAccount(data.IdSender).ConfigureAwait(false);
        var receiver = await _accountRepository.GetAccount(data.IdReceiver).ConfigureAwait(false);

        if (sender is null || receiver is null)
            throw new UserNotFoundException("Conta(s) não encontrada(s)");

        IsBalanceSufficient(sender, value);
    }

}

