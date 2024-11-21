using System.ComponentModel.DataAnnotations;

namespace Bank.Bank.Application.DTOs.Request;

public class AddBalanceDto
{
    [Range(0, double.MaxValue,ErrorMessage = "Value must be greater than zero")]
    public decimal Value { get; set; }
}