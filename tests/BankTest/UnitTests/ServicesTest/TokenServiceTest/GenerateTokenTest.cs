using Bank.Bank.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BankTest.UnitTests.ServicesTest.TokenServiceTest;

public class GenerateTokenTest
{
    [Fact]
    public void ShouldReturnTokenJwt()
    {
        var user = new User()
        {
            Id = Guid.NewGuid(),
            UserName = "test",
            Email = "test@test.com",
        };
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("https://localhost:5001");
        configurationMock.Setup(x => x["Jwt:SecreteKey"]).Returns("23b238f23u2983bf283f8");
        var httpContext = new Mock<IHttpContextAccessor>();
        var token = new TokenService(httpContext.Object);
        
        var result = token.GenerateToken(user);
        
        Assert.NotNull(result);
    }
}