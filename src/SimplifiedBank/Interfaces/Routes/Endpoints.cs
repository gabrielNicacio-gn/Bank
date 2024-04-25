
using Microsoft.AspNetCore.Mvc;
using SimplifiedBank.Application.DTOs;
using SimplifiedBank.Application.DTOs.Response;
using SimplifiedBank.Domain.Repositories;
using SimplifiedBank.Interfaces.Exceptions;

namespace SimplifiedBank.Interfaces.Routes
{
    public static class Endpoints
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapGet("/get/{id}", async (int id, [FromServices] IReturnAccount _returnAccount) =>
            {
                try
                {
                    var account = await _returnAccount.GetAccount(id);
                    var viewAccount = new GetAccountData(account.Id, account.FullName, account.Balance);
                    return Results.Ok(viewAccount);
                }
                catch (UserNotFoundException)
                {
                    return Results.NotFound();
                }
            });

            app.MapPost("/post", async ([FromServices] ICreateTransaction _createTransaction,
            [FromBody] TransactioCreationData data) =>
            {
                try
                {
                    var newTransaction = await _createTransaction.Create(data);
                    return Results.Ok(newTransaction);
                }
                catch (UserNotFoundException)
                {
                    return Results.NotFound();
                }
                catch (UnauthorizedTransactionException)
                {
                    return Results.Unauthorized();
                }
                catch (InsufficienteBalanceException)
                {
                    return Results.UnprocessableEntity();
                }
            });
        }
    }
}