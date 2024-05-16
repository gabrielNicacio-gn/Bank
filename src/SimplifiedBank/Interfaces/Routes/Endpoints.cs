
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

            endpoints.MapPost("/login", [AllowAnonymous] async ([FromServices] IConfiguration _config, [FromServices] TokenService _token,
            string email,
            string password) =>
            {
                LoginDTO login = new LoginDTO(email, password);
                var newToken = await _token.GenerateToken(login, _config);
                if (newToken is null)
                    return Results.BadRequest("Falha ao gerar token");

                return Results.Ok(newToken);
            });

            endpoints.MapGet("/{id}/account", async ([FromRoute] int id, [FromServices] IReturnAccount _returnAccount) =>
            {
                try
                {
                    var account = await _returnAccount.GetAccount(id);
                    return Results.Ok(account);
                }
                catch (UserNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            }).RequireAuthorization();

            endpoints.MapPost("/transaction", async ([FromServices] ICreateTransaction _createTransaction,
            [FromBody] TransactionCreationData data) =>
            {
                try
                {
                    var newTransaction = await _createTransaction.Create(data);
                    return Results.Ok(newTransaction);
                }
                catch (UserNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (InvalidTransactionException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (InsufficienteBalanceException ex)
                {
                    return Results.UnprocessableEntity(ex.Message);
                }
            }).RequireAuthorization();
        }
    }
}