using System.Linq.Expressions;
using System.Security.Claims;
using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Application.Exceptions;
using Bank.Bank.Application.Services;
using Bank.Bank.Infrastructure.Data;
using Bank.Bank.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BankTest.UnitTests.ServicesTest;

public class AccountServicesTest
{
    [Fact]
    public async Task  ShouldReturnAccount()
    {
       var idUser = Guid.NewGuid();
       var expectedAccount = new Account(){UserId = idUser,NumberAccount = 78093};
       var expectedAccountResponse = new AccountResponse()
           {IdAccount = expectedAccount.AccountId, NumberAccount = expectedAccount.NumberAccount, Balance = expectedAccount.Balance};
       var accountRepositoryMock = new Mock<IAccountRepository>();
       accountRepositoryMock.Setup(x => x.GetAccountById(idUser)).ReturnsAsync(expectedAccount);
       
       var accountService = new AccountServices(accountRepositoryMock.Object);
       var result = await accountService.GetAccount(idUser);
       
       Assert.Equal(expectedAccountResponse.IdAccount,result.IdAccount);
       Assert.Equal(expectedAccountResponse.NumberAccount,result.NumberAccount);
       Assert.Equal(expectedAccountResponse.Balance,result.Balance);
    }
    [Fact]
    public async Task  ShouldGenerationAccountNotExistException()
    {
        var idUser = Guid.NewGuid();
        var exceptionExpected = new AccountNotExistException("Account not found");
        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(x => x.GetAccountById(It.IsAny<Guid>())).ReturnsAsync((Account)null);
       
        var accountService = new AccountServices(accountRepositoryMock.Object);
        
        var exception = Assert.ThrowsAsync<AccountNotExistException>
            (async ()=>await accountService.GetAccount(idUser));
        var result = await exception;
        Assert.Equal(exceptionExpected.Message,result.Message);
    }
    [Fact]
    public async Task ShouldAddBalanceAccount()
    {
        var idUser = Guid.NewGuid();
        var addBalanceDto = new AddBalanceDto()
        {
            Value = 456.9m
        };
        var expectedResult = new AccountResponse()
        {
            Balance = addBalanceDto.Value
        };
        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(a => a.UpdateBalance(idUser, addBalanceDto.Value)).ReturnsAsync(1);
        var accountService = new AccountServices(accountRepositoryMock.Object);
        
        var result = await accountService.AddBalance(addBalanceDto,idUser);
        
        Assert.Equal(expectedResult.Balance,result.Balance);
    }
    [Fact]
    public async Task ShouldGenerateFailUpdateBalanceException()
    {
        var idUser = Guid.NewGuid();
        var addBalanceDto = new AddBalanceDto()
        {
            Value = 456.9m
        };
        var exceptionExpected = new FailUpdateBalanceException("Failed to update balance");
        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(a => a.UpdateBalance(idUser, addBalanceDto.Value)).ReturnsAsync(0);
        var accountService = new AccountServices(accountRepositoryMock.Object);
        
        var exception = Assert.ThrowsAsync<FailUpdateBalanceException>
            (async ()=>await accountService.AddBalance(addBalanceDto,idUser));
        var result = await exception;
        Assert.Equal(exceptionExpected.Message,result.Message);
    }
}