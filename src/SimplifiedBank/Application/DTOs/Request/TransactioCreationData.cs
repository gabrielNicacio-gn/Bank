
namespace SimplifiedBank.Application.DTOs.Request;
public record TransactionCreationData(int IdSender, int IdReceiver, decimal Value);
