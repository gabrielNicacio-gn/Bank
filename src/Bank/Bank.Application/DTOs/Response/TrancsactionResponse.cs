namespace Bank.Bank.Application.DTOs.Response;

public class TransactionResponse
{
    public Guid IdTransaction { get; set; }
    public decimal Value{ get; set; }
    public DateTime TimeStamp { get; set; }
    public Guid IdSender { get; set; }
    public Guid IdReceiver { get; set; }
}