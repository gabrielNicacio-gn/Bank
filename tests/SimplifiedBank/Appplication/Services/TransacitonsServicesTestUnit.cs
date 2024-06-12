
using Dtos = SimplifiedBank.Application.DTOs;
using Repositories = SimplifiedBank.Infrastructure.Repositories;
using Xunit;
using Moq;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Interfaces.Exceptions;
using SimplifiedBank.Application.Services;

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

            transactionServices.SetupSequence(ts => ts.ValidateTransaction(newTransaction));
            var validate = () => transactionServices.Object.ValidateTransaction(newTransaction);
            transactionServices.Verify();
        }
        [Fact]
        public void ShouldReturnAnInsufficientBalanceException()
        {
            var newTransaction = new Dtos::Request.TransactionCreationData(1, 2, 1100);
            var accountRepositoriesMock = new Mock<Repositories::AccountsRepositories.IAccountRepositories>();
            var accountSender = new Account(1, "testOne", "testOne@example.com", "12345678909", "test@One", 500);
            var accountReceiver = new Account(2, "testTwo", "testTwo@example.com", "98765432112", "test@Two", 100);
            var transactionServices = new Mock<ITransactionsServices>();
            var expectedException = new InsufficienteBalanceException("Saldo Insuficiente");

            transactionServices.Setup(ts => ts.ValidateTransaction(newTransaction))
            .Throws(new InsufficienteBalanceException("Saldo Insuficiente"));

            var exception = () => transactionServices.Object.ValidateTransaction(newTransaction);

            var result = Assert.Throws<InsufficienteBalanceException>(exception);

            Assert.Equal(expectedException.Message, result.Message);
        }

        [Fact]
        public void ShouldReturnAnUserNotFoundException()
        {
            var newTransaction = new Dtos::Request.TransactionCreationData(1, 2, 1100);
            var accountRepositoriesMock = new Mock<Repositories::AccountsRepositories.IAccountRepositories>();
            var accountSender = new Account(1, "testOne", "testOne@example.com", "12345678909", "test@One", 500);
            var accountReceiver = new Account(2, "testTwo", "testTwo@example.com", "98765432112", "test@Two", 100);
            var transactionServices = new Mock<ITransactionsServices>();
            var expectedException = new UserNotFoundException("Conta(s) não encontrada(s)");

            transactionServices.Setup(ts => ts.ValidateTransaction(newTransaction))
            .Throws(new UserNotFoundException("Conta(s) não encontrada(s)"));

            var exception = () => transactionServices.Object.ValidateTransaction(newTransaction);

            var result = Assert.Throws<UserNotFoundException>(exception);

            Assert.Equal(expectedException.Message, result.Message);

        }
    }
}