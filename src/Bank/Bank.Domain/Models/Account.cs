namespace Bank.Bank.Domain.Models;

public class Account
{
    public Guid AccountId { get;private set; }
    public decimal Balance { get; private set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Account(decimal balance)
    {
        AccountId = Guid.NewGuid();
        Balance = balance;
    }
}