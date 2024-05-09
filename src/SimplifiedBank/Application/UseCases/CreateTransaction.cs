
using SimplifiedBank.Domain.Interface;
using Repositories = SimplifiedBank.Infrastructure.Repositories;
using SimplifiedBank.Services.Interfaces;
using SimplifiedBank.Interfaces.Exceptions;
using Entities = SimplifiedBank.Domain.Entities;
using DataDTOs = SimplifiedBank.Application.DTOs;

namespace SimplifiedBank.Application.UseCases
{
    public class CreateTransaction : ICreateTransaction
    {
        private readonly Repositories::TransactionRepositories.ITransactionRepository _transactionRepository;
        private readonly IValidationTransaction _validation;
        public CreateTransaction(Repositories::TransactionRepositories.ITransactionRepository transactionRepository,
        Repositories::AccountsRepositories.IAccountRepositories accountRepository,
        IValidationTransaction validation)
        {
            _transactionRepository = transactionRepository;
            _validation = validation;
        }
        public async Task<DataDTOs::Response.TransactionCreationResponseData> Create(DataDTOs::Request.TransactioCreationData data)
        {
            var newTransaction = new Entities::Transaction(data.Value, data.IdSender, data.IdReceiver);
            await _transactionRepository.CreateTransaction(newTransaction).ConfigureAwait(false);

            var IsValid = await _validation.Validation().ConfigureAwait(false);

            if (!IsValid)
                throw new UnauthorizedAccessException("Transação não validada");

            var newTransactionResult = new DataDTOs::Response.TransactionCreationResponseData(newTransaction.IdSender, newTransaction.IdReceiver);
            return newTransactionResult;
            // Chamar serviço notificador
        }
    }
}