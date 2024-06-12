
namespace SimplifiedBank.Application.DTOs.Response
{
    public record ResponseDataForTransactionCreation(int IdSender, int IdReceiver, decimal Value);
}