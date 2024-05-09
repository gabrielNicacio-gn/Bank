
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplifiedBank.Application.Authentication;
using SimplifiedBank.Application.DTOs.Login;
using SimplifiedBank.Application.DTOs.Request;
using SimplifiedBank.Application.DTOs.Response;
using SimplifiedBank.Domain.Interface;
using SimplifiedBank.Interfaces.Exceptions;

namespace SimplifiedBank.Interfaces.Routes
{
    public static class Endpoints
    {
        public static void MapEndpoints(this WebApplication app)
        {
            var endpoints = app.MapGroup("/bank").WithTags("Bank");


            endpoints.MapGet("/token", ([FromServices] IConfiguration _config, [FromServices] TokenService _token, string email, string password) =>
            {
                var login = new LoginDTO(email, password);
                var newToken = _token.GenerateToken(login, _config);
                if (newToken is null)
                    return Results.BadRequest("Falha ao gerar token");

                return Results.Ok(newToken);
            });

            endpoints.MapGet("/{id}/account", async ([FromRoute] int id, [FromServices] IReturnAccount _returnAccount) =>
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

            endpoints.MapPost("/transaction", async ([FromServices] ICreateTransaction _createTransaction,
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
                catch (InvalidTransactionException)
                {
                    return Results.BadRequest();
                }
                catch (InsufficienteBalanceException)
                {
                    return Results.UnprocessableEntity();
                }
            }).RequireAuthorization();
        }
    }
}