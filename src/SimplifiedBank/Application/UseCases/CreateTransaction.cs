
using SimplifiedBank.Domain.Interface;
using Repositories = SimplifiedBank.Infrastructure.Repositories;
using SimplifiedBank.Services.Interfaces;
using SimplifiedBank.Interfaces.Exceptions;
using Entities = SimplifiedBank.Domain.Entities;
using DataDTOs = SimplifiedBank.Application.DTOs;
using SimplifiedBank.Application.Services;

namespace SimplifiedBank.Application.UseCases;

public class CreateTransaction : ICreateTransaction
{
    private readonly Repositories::TransactionRepositories.ITransactionRepository _transactionRepository;
    private readonly IServiceExternalTransactionAuthorizer _externalAuthorizer;
    private readonly ITransactionsServices _transactionServices;
    public CreateTransaction(Repositories::TransactionRepositories.ITransactionRepository transactionRepository,
    IServiceExternalTransactionAuthorizer externalAuthorizer, ITransactionsServices transactionServices)
    {
        _transactionRepository = transactionRepository;
        _externalAuthorizer = externalAuthorizer;
        _transactionServices = transactionServices;
    }
    public async Task<DataDTOs::Response.ResponseDataForTransactionCreation> Create(DataDTOs::Request.TransactionCreationData data)
    {
        await _transactionServices.ValidateTransaction(data);
        await ValidateExternal();
        var newTransaction = new Entities::Transaction(data.Value, data.IdSender, data.IdReceiver);
        await _transactionRepository.CreateTransaction(newTransaction).ConfigureAwait(false);

        var newTransactionResult = new DataDTOs::Response.ResponseDataForTransactionCreation(newTransaction.IdSender, newTransaction.IdReceiver, newTransaction.Value);
        return newTransactionResult;

    }
    private async Task<bool> ValidateExternal()
    {
        var IsValid = await _externalAuthorizer.Authorizer().ConfigureAwait(false);

        if (!IsValid)
            throw new InvalidTransactionException("Transação Invalida");

        return IsValid;
    }
}
