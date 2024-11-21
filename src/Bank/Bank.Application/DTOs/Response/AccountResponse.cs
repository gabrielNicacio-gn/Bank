using Bank.Bank.Domain.Models;

namespace Bank.Bank.Application.DTOs.Response;

public class AccountResponse
{
    public decimal Balance { get; set;}
    public Guid IdAccount { get; set;}
    public int NumberAccount { get; set;}
}