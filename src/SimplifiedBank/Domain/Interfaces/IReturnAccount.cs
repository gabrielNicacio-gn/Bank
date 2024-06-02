
using SimplifiedBank.Application.DTOs.Response;

namespace SimplifiedBank.Domain.Interface;

public interface IReturnAccount
{
        GetAccountData GetAccount(int id);
}