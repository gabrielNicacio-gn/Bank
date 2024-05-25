using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Moq;
using SimplifiedBank.Application.DTOs.Response;
using SimplifiedBank.Application.UseCases;
using Entities = SimplifiedBank.Domain.Entities;
using SimplifiedBank.Infrastructure.Repositories.TransactionRepositories;
using Xunit;

namespace SimplifiedBank.Appplication.UseCaseTest.ReturnTheLatestTransactions
{
    public class ReturnTheLatestTransactionsTestUnit
    {
        [Fact]
        public async Task ItMustReturnAListOfTheLatestTransactionsForAnAccount()
        {
            var idAccount = 1;
            var transacitonRepositoriesMock = new Mock<ITransactionRepository>();
            var firstTransaction = new Entities::Transaction(idSender: 2, idReceiver: 1, value: 50);
            var secondTransaction = new Entities::Transaction(idSender: 2, idReceiver: 1, value: 50);
            var transactions = new List<Entities::Transaction>()
            {
                firstTransaction,secondTransaction
            };
            var expectedTransactions = new List<GetLatestTransactions>()
            {
                new GetLatestTransactions(0,secondTransaction.IdSender,secondTransaction.IdReceiver,secondTransaction.Value,DateTime.UtcNow),
                new GetLatestTransactions(0,firstTransaction.IdSender,firstTransaction.IdReceiver,firstTransaction.Value,DateTime.UtcNow)
            };

            transacitonRepositoriesMock.Setup(tr => tr.GetLatestTransaction(idAccount))
            .ReturnsAsync(transactions.AsEnumerable);

            var returnLatest = new ReturnsTheLatestTransactions(transacitonRepositoriesMock.Object);
            var result = await returnLatest.GetList(idAccount);

            Assert.Equal(expectedTransactions.Count, result.Count());
        }
    }
}