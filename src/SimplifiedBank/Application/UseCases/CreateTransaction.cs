
using SimplifiedBank.Application.DTOs;
using SimplifiedBank.Domain.Repositories;
using SimplifiedBank.Infrastructure.Repositories;
using SimplifiedBank.Services.Repositories;
using SimplifiedBank.Interfaces.Exceptions;
using SimplifiedBank.Application.DTOs.Response;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.UseCases
{
    public class CreateTransaction : ICreateTransaction
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IReturnAccount _returnAccount;
        private readonly IAuthorizationTransaction _authorization;
        public CreateTransaction(ITransactionRepository transactionRepository, IReturnAccount returnAccount,
        IAuthorizationTransaction authorization)
        {
            _transactionRepository = transactionRepository;
            _authorization = authorization;
            _returnAccount = returnAccount;
        }
        public async Task<TransactionCreationResponseData> Create(TransactioCreationData data)
        {
            var accountSender = await _returnAccount.GetAccount(data.IdSender);
            var accountReceiver = await _returnAccount.GetAccount(data.IdReceiver);

            var exist = accountSender is not null && accountReceiver is not null
            ? await _authorization.Authorization(accountSender)
            : throw new UserNotFoundException();

            if (!exist)
                throw new UnauthorizedTransactionException();
            UserBalanceIsSufficient(accountSender, data.Value);

            var newTransaction = new Domain.Entities.Transaction(data.Value, data.IdSender, data.IdReceiver);
            await _transactionRepository.CreateTransaction(newTransaction);

            return new TransactionCreationResponseData(newTransaction.IdSender, newTransaction.IdReceiver);
            // Chamar serviço notificador
        }
        private void UserBalanceIsSufficient(Account accountSender, decimal value)
        {
            if (accountSender.Balance < value)
                throw new InsufficienteBalanceException();
        }

    }
}