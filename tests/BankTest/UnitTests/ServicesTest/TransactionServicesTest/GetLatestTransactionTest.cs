namespace BankTest.UnitTests.ServicesTest.TransactionServicesTest;

public class GetLatestTransactionTest
{
    [Fact]
    public async Task ShouldReturnAllLatestTransactions()
    {
        var idSender = Guid.NewGuid();
        var idReceiver = Guid.NewGuid();
        var listTransaction = new List<Transaction>()
        {
            new Transaction(){IdSender = idSender,IdReceiver = idReceiver,Value = 200},
            new Transaction(){IdSender = idSender,IdReceiver = idReceiver,Value = 100}
        };
        var expectedResult = listTransaction;
            
        var transactionRepositoryMock = new Mock<ITransactionRepository>();
        transactionRepositoryMock.Setup(t=>t.GetLatestTransactions(idSender))
            .ReturnsAsync(listTransaction);
        var transactionServices = new TransactionServices(transactionRepositoryMock.Object,null);
            
        var result = await transactionServices.GetLatestTransactions(idSender);
        var countTransactions = result.Count();
        var firstTransaction = result.ToArray().First();
            
        Assert.Equal(firstTransaction.IdTransaction, listTransaction[0].Id);
        Assert.Equal(2, countTransactions);

    }
}