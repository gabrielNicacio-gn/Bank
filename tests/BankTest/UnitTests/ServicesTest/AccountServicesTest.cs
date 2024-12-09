using System.Security.Claims;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Application.Services;
using Bank.Bank.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BankTest.UnitTests.ServicesTest;

public class AccountServicesTest
{
    [Fact]
    public void ShouldCreateAccount()
    {
       var idUser = Guid.NewGuid();
       var expectedAccount = new Account(){UserId = idUser,NumberAccount = 78093};
       var expectedAccountResponse = new AccountResponse()
           {IdAccount = expectedAccount.AccountId, NumberAccount = expectedAccount.NumberAccount, Balance = expectedAccount.Balance};
       var httpContext = new Mock<HttpContext>()
           .Setup(http=>http.User.FindFirstValue(ClaimTypes.NameIdentifier))
           .Returns(idUser.ToString());
       var httpContextAccessorMock = new Mock<IHttpContextAccessor>()
               .Setup(httpContextAccessor => httpContextAccessor.HttpContext)
           ;
           
       var bankContext = new Mock<BankContext>();
       
       var accountService = new AccountServices(bankContext.Object, httpContextAccessorMock.Object);
       var result = accountService.GetAccount().Result;
       
       Assert.Equal(expectedAccountResponse,result);
    }
}