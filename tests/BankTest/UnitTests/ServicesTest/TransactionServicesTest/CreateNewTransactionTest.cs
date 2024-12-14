namespace BankTest.UnitTests.ServicesTest.TransactionServicesTest;

public class CreateNewTransactionTest
{
    [Fact]
    public async Task ShouldCreateNewTransaction()
        {
            var idSender = Guid.NewGuid();
            var idReceiver = Guid.NewGuid();
            var newTransactionDto = new CreateNewTransactionDto()
            {
                NumberAccountReceiver = 74658,
                Value = 2000,
            };
            var accountSender = new Account()
            {
                UserId = idSender,
                Balance = 6000,
                NumberAccount = 67383
            };
            var accountReceiver = new Account()
            {
                UserId = idReceiver,
                Balance = 1000,
                NumberAccount = newTransactionDto.NumberAccountReceiver 
            };
            var transactionCreated = new Transaction()
            {
                IdSender = idSender,
                IdReceiver = idReceiver,
                Value = newTransactionDto.Value,
            };
            var expectedResult = new TransactionResponse()
            {
                IdSender = transactionCreated.IdSender,
                IdReceiver = transactionCreated.IdReceiver,    
                Value = newTransactionDto.Value,
                TimeStamp = DateTime.UtcNow,
            };
         
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(a=>a.GetAccountById(It.IsAny<Guid>())).ReturnsAsync(accountSender);
            accountRepositoryMock.Setup(a=>
                a.GetAccountByNumberAccount(It.IsAny<int>())).ReturnsAsync(accountReceiver);
            var transactionRepositoryMock = new Mock<ITransactionRepository>();
            transactionRepositoryMock.Setup(t => t.CreateTransaction(It.IsAny<Transaction>())).Returns(Task.CompletedTask);
            transactionRepositoryMock.Setup(t => t.UpdateBalanceAccount
                (It.IsAny<Guid>(),It.IsAny<Guid>(),newTransactionDto.Value)).ReturnsAsync(true);
            var transactionServices = new TransactionServices(transactionRepositoryMock.Object,accountRepositoryMock.Object);
            
            var result = await transactionServices.CreateNewTransaction(newTransactionDto,idSender);
            
            Assert.Equal(expectedResult.Value, result.Value);   
        }
        
        [Fact]
        public async Task ShouldGenerateInsufficientBalanceException()
        {
            var idSender = Guid.NewGuid();
            var newTransactionDto = new CreateNewTransactionDto()
            {
                NumberAccountReceiver = 74658,
                Value = 8000,
            };
            var accountSender = new Account()
            {
                UserId = idSender,
                Balance = 6000,
                NumberAccount = 67383
            };
            var expectedResult = new InsufficientBalanceException("Insufficient balance");
         
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(a=>a.GetAccountById(It.IsAny<Guid>())).ReturnsAsync(accountSender);
            var transactionRepositoryMock = new Mock<ITransactionRepository>();
         
            var transactionServices = new TransactionServices(transactionRepositoryMock.Object,accountRepositoryMock.Object);
            
            var exception = Assert.ThrowsAsync<InsufficientBalanceException>(() => 
                 transactionServices.CreateNewTransaction(newTransactionDto,idSender));
            var result = await exception;
            Assert.Equal(expectedResult.Message,result.Message);
        }
        
        [Fact]
        public async Task ShouldGenerateAccountNotFoundException()
        {
            var idSender = Guid.NewGuid();
            var newTransactionDto = new CreateNewTransactionDto()
            {
                NumberAccountReceiver = 74658,
                Value = 2000,
            };
            var accountSender = new Account()
            {
                UserId = idSender,
                Balance = 6000,
                NumberAccount = 67383
            };
            
            var expectedResult = new AccountNotFoundException("Account not found");
         
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(a=>a.GetAccountById(It.IsAny<Guid>())).ReturnsAsync(accountSender);
            accountRepositoryMock.Setup(a=>
                a.GetAccountByNumberAccount(It.IsAny<int>())).ReturnsAsync((Account)null);
            var transactionRepositoryMock = new Mock<ITransactionRepository>();
      
            var transactionServices = new TransactionServices(transactionRepositoryMock.Object,accountRepositoryMock.Object);

            var exception = Assert.ThrowsAsync<AccountNotFoundException>
                (()=>transactionServices.CreateNewTransaction(newTransactionDto, idSender));
            var result = await exception;
            
            Assert.Equal(expectedResult.Message, result.Message);   
        }
        
        [Fact]
        public async Task ShouldGenerateTransactionBetweenAccountsFailsException()
        {
            var idSender = Guid.NewGuid();
            var idReceiver = Guid.NewGuid();
            var newTransactionDto = new CreateNewTransactionDto()
            {
                NumberAccountReceiver = 74658,
                Value = 2000,
            };
            var accountSender = new Account()
            {
                UserId = idSender,
                Balance = 6000,
                NumberAccount = 67383
            };
            var accountReceiver = new Account()
            {
                UserId = idReceiver,
                Balance = 1000,
                NumberAccount = newTransactionDto.NumberAccountReceiver 
            };
            
            var expectedResult = new TransactionBetweenAccountsFailsException("Error in transaction");
         
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(a=>a.GetAccountById(It.IsAny<Guid>())).ReturnsAsync(accountSender);
            accountRepositoryMock.Setup(a=>
                a.GetAccountByNumberAccount(It.IsAny<int>())).ReturnsAsync(accountReceiver);
            var transactionRepositoryMock = new Mock<ITransactionRepository>();
            transactionRepositoryMock.Setup(t => t.CreateTransaction(It.IsAny<Transaction>())).Returns(Task.CompletedTask);
            transactionRepositoryMock.Setup(t => t.UpdateBalanceAccount
            (It.IsAny<Guid>(),It.IsAny<Guid>(),newTransactionDto.Value)).ReturnsAsync(false);
            var transactionServices = new TransactionServices(transactionRepositoryMock.Object,accountRepositoryMock.Object);

            var exception = Assert.ThrowsAsync<TransactionBetweenAccountsFailsException>
                (()=>transactionServices.CreateNewTransaction(newTransactionDto, idSender));
            var result = await exception;
            
            Assert.Equal(expectedResult.Message, result.Message);   
        }

}