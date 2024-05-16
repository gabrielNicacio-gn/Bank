
using Dtos = SimplifiedBank.Application.DTOs;
using Repositories = SimplifiedBank.Infrastructure.Repositories;
using Entities = SimplifiedBank.Domain.Entities;
using UseCase = SimplifiedBank.Application.UseCases;
using Xunit;
using Moq;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Services.Interfaces;
using SimplifiedBank.Interfaces.Exceptions;
using SimplifiedBank.Application.Services;
using SimplifiedBank.Application.UseCases;



namespace SimpliffiedBankTest.Appplication.UseCaseTest.CreateTransactionTest
{
    public class CreateTransactionTestUnit
    {
        [Theory]
        [InlineData(1, 2, 50)]
        [InlineData(1, 2, 100)]
        [InlineData(1, 2, 150)]
        public async Task CreatesTransactionSucessAndReturnsResponseDataForTransactionCreation(int idSender, int idReceiver, decimal value)
        {
            var newTransaction = new Dtos::Request.TransactionCreationData(idSender, idReceiver, value);

            var expectedTransaction = new Dtos::Response.ResponseDataForTransactionCreation(IdSender: newTransaction.IdSender,
            IdReceiver: newTransaction.IdReceiver,
            Value: newTransaction.Value);

            var transactionRepositoriesMock = new Mock<Repositories::TransactionRepositories.ITransactionRepository>();
            var validationTransaciton = new Mock<ITransactionsServices>();
            var externalAuthorizerMock = new Mock<IServiceExternalTransactionAuthorizer>();
            var accountSender = new Account(1, "testOne", "testOne@example.com", "12345678909", "test@One", 500);
            var accountReceiver = new Account(2, "testTwo", "testTwo@example.com", "98765432112", "test@Two", 100);
            // When
            transactionRepositoriesMock.Setup(tc => tc.CreateTransaction(It.IsAny<Entities::Transaction>()));
            validationTransaciton.Setup(vl => vl.ValidateTransaction(newTransaction));
            externalAuthorizerMock.Setup(auth => auth.Authorizer()).ReturnsAsync(true);

            var CreateTransaction = new CreateTransaction(transactionRepositoriesMock.Object,
            externalAuthorizerMock.Object, validationTransaciton.Object);

            var result = await CreateTransaction.Create(newTransaction);

            Assert.Equal(expectedTransaction, result);
        }

        /*
                [Fact]
                public async Task ShouldReturnAnUnauthorizedTransactionException()
                {
                    var newTransaction = new Dtos::Request.TransactionCreationData(1, 2, 100);

                    var expectedTransaction = new Dtos::Response.ResponseDataForTransactionCreation(IdSender: newTransaction.IdSender,
                    IdReceiver: newTransaction.IdReceiver,
                    Value: newTransaction.Value);

                    var transactionRepositoriesMock = new Mock<Repositories::TransactionRepositories.ITransactionRepository>();
                    var validationTransaciton = new Mock<ITransactionsServices>();
                    var externalAuthorizerMock = new Mock<IServiceExternalTransactionAuthorizer>();
                    var accountSender = new Account(1, "testOne", "testOne@example.com", "12345678909", "test@One", 500);
                    var accountReceiver = new Account(2, "testTwo", "testTwo@example.com", "98765432112", "test@Two", 100);
                    var message = "Transação Invalida";

                    transactionRepositoriesMock.Setup(tc => tc.CreateTransaction(It.IsAny<Entities::Transaction>()));
                    validationTransaciton.Setup(vl => vl.ValidateTransaction(newTransaction));
                    externalAuthorizerMock.Setup(auth => auth.Authorizer()).ReturnsAsync(false);

                    var CreateTransaction = new CreateTransaction(transactionRepositoriesMock.Object,
                    externalAuthorizerMock.Object, validationTransaciton.Object);

                    var result = async () => await CreateTransaction.Create(newTransaction);

                    var exception = await Assert.ThrowsAsync<InvalidTransactionException>(result);

                    Assert.Equal(message, exception.Message);
                }
        */
    }
}