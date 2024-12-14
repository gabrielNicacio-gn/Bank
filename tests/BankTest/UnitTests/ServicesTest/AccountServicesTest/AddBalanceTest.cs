namespace BankTest.UnitTests.ServicesTest.AccountServicesTest;

public class AddBalanceTest
{
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