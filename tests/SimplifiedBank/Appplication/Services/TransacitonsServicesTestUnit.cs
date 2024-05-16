
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


namespace SimplifiedBank.Appplication.Services
{
    public class TransacitonsServicesTests
    {
        [Fact]
        public void WellSecededTransactionValidation()
        {
            var newTransaction = new Dtos::Request.TransactionCreationData(1, 2, 100);
            var accountRepositoriesMock = new Mock<Repositories::AccountsRepositories.IAccountRepositories>();
            var accountSender = new Account(1, "testOne", "testOne@example.com", "12345678909", "test@One", 500);
            var accountReceiver = new Account(2, "testTwo", "testTwo@example.com", "98765432112", "test@Two", 100);
            var transactionServices = new Mock<ITransactionsServices>();

            transactionServices.Setup(ts => ts.ValidateTransaction(newTransaction)).Returns(Task.CompletedTask);

            Assert.True(transactionServices.Object.ValidateTransaction(newTransaction).IsCompleted);
        }

        [Fact]
        public async void ShouldReturnAnInsufficientBalanceException()
        {
            var newTransaction = new Dtos::Request.TransactionCreationData(1, 2, 1100);
            var accountRepositoriesMock = new Mock<Repositories::AccountsRepositories.IAccountRepositories>();
            var accountSender = new Account(1, "testOne", "testOne@example.com", "12345678909", "test@One", 500);
            var accountReceiver = new Account(2, "testTwo", "testTwo@example.com", "98765432112", "test@Two", 100);
            var transactionServices = new Mock<ITransactionsServices>();
            var expectedException = new InsufficienteBalanceException("Saldo Insuficiente");

            transactionServices.Setup(ts => ts.ValidateTransaction(newTransaction))
            .Returns(() => throw new InsufficienteBalanceException("Saldo Insuficiente"));

            var exception = async () => await transactionServices.Object.ValidateTransaction(newTransaction);

            var result = await Assert.ThrowsAsync<InsufficienteBalanceException>(exception);

            Assert.Equal(expectedException.Message, result.Message);

        }

        [Fact]
        public async void ShouldReturnAnUserNotFoundException()
        {
            var newTransaction = new Dtos::Request.TransactionCreationData(1, 2, 1100);
            var accountRepositoriesMock = new Mock<Repositories::AccountsRepositories.IAccountRepositories>();
            var accountSender = new Account(1, "testOne", "testOne@example.com", "12345678909", "test@One", 500);
            var accountReceiver = new Account(2, "testTwo", "testTwo@example.com", "98765432112", "test@Two", 100);
            var transactionServices = new Mock<ITransactionsServices>();
            var expectedException = new UserNotFoundException("Conta(s) não encontrada(s)");

            transactionServices.Setup(ts => ts.ValidateTransaction(newTransaction))
            .Returns(() => throw new UserNotFoundException("Conta(s) não encontrada(s)"));

            var exception = async () => await transactionServices.Object.ValidateTransaction(newTransaction);

            var result = await Assert.ThrowsAsync<UserNotFoundException>(exception);

            Assert.Equal(expectedException.Message, result.Message);

        }
    }
}