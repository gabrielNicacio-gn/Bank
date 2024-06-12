
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
    public DataDTOs::Response.ResponseDataForTransactionCreation Create(DataDTOs::Request.TransactionCreationData data)
    {
        _transactionServices.ValidateTransaction(data);
        _externalAuthorizer.Authorizer();
        var newTransaction = new Entities::Transaction(data.Value, data.IdSender, data.IdReceiver);
        _transactionRepository.CreateTransaction(newTransaction);

        var newTransactionResult = new DataDTOs::Response.ResponseDataForTransactionCreation(newTransaction.IdSender, newTransaction.IdReceiver, newTransaction.Value);
        return newTransactionResult;
        //Notification 

    }
}
