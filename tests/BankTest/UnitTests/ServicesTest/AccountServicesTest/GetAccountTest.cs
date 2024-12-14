using Bank.Bank.Infrastructure.Interfaces;

namespace BankTest.UnitTests.ServicesTest.AccountServicesTest;

public class GetAccountTest
{
    [Fact]
    public async Task ShouldReturnAccount()
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
}