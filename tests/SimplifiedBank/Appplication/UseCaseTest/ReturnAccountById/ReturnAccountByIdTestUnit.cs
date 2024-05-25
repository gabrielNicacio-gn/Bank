using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SimplifiedBank.Application.DTOs.Response;
using SimplifiedBank.Application.UseCases;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Interface;
using SimplifiedBank.Infrastructure.Repositories.AccountsRepositories;
using SimplifiedBank.Interfaces.Exceptions;
using Xunit;

namespace SimplifiedBank.Appplication.UseCaseTest.ReturnAccountById
{
    public class ReturnAccountByIdTestUnit
    {
        [Theory]
        [InlineData(1)]
        public async Task MustReturnAnAccountWithTheSpecifiedId(int id)
        {
            var expectedAccountOne = new GetAccountData(1, "User One", 100);
            var expectedAccountTwo = new GetAccountData(2, "User Two", 100);
            var AccountOne = new Account(1, "User One", "userone@example.com", "23456789010", "user123", 100);
            var AccountTwo = new Account(2, "User Two", "usertwo@example.com", "09876543211", "user456", 100);
            var AccountRepositoriesMock = new Mock<IAccountRepositories>();

            AccountRepositoriesMock.Setup(ac => ac.GetAccountById(id))
            .ReturnsAsync(AccountOne);

            var returnAccount = new ReturnAccount(AccountRepositoriesMock.Object);

            var result = await returnAccount.GetAccount(id);

            Assert.Equal(result, expectedAccountOne);

        }
        [Fact]
        public async Task ShouldReturnAUserNotFoundException()
        {
            var exceptionExpected = new UserNotFoundException("Conta(s) não encontrada(s)");
            var ReturnAccountMock = new Mock<IReturnAccount>();

            ReturnAccountMock.Setup(re => re.GetAccount(It.IsAny<int>()))
            .Returns(() => throw new UserNotFoundException("Conta(s) não encontrada(s)"));

            var exception = async () => await ReturnAccountMock.Object.GetAccount(It.IsAny<int>());

            var result = await Assert.ThrowsAsync<UserNotFoundException>(exception);

            Assert.Equal(exceptionExpected.Message, result.Message);

        }
    }
}