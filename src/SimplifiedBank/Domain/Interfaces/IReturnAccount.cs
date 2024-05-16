
using SimplifiedBank.Application.DTOs.Response;

namespace SimplifiedBank.Domain.Interface;

public interface IReturnAccount
{
        Task<GetAccountData> GetAccount(int id);
}