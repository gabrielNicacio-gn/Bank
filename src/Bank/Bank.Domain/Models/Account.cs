using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Bank.Domain.Models;

public class Account
{
    [Column("id_account")]
    public Guid AccountId { get;private set; }
    [Column("number_account")]
    public int NumberAccount { get;set; }
    [Column("balance")]
    public decimal Balance { get; set; }
    [Column("user_id")]
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Account()
    {
        AccountId = Guid.NewGuid();
        NumberAccount = DateTime.UtcNow.Hour + new Random().Next(1000, 100000);
    }
}