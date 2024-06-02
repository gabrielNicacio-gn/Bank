
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplifiedBank.Application.Authentication;
using SimplifiedBank.Application.DTOs.Login;
using SimplifiedBank.Application.DTOs.Request;
using SimplifiedBank.Application.DTOs.Response;
using SimplifiedBank.Application.UseCases;
using SimplifiedBank.Domain.Interface;
using SimplifiedBank.Domain.Interfaces;
using SimplifiedBank.Interfaces.Exceptions;

namespace SimplifiedBank.Interfaces.Routes
{
    public static class TransactionEndpoints
    {
        public static void MapTransactionEndpoints(this WebApplication app)
        {
            var endpoints = app.MapGroup("/bank").WithTags("Bank");

            endpoints.MapPost("/transaction", ([FromBody] TransactionCreationData data, [FromServices] ICreateTransaction _create) =>
            {
                try
                {
                    var created = _create.Create(data);
                    return Results.Created<ResponseDataForTransactionCreation>("", created);
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

            endpoints.MapGet("{id}/latest", ([FromRoute] int id, [FromServices] IReturnTheLatestTransactions _latestTransaction) =>
            {
                try
                {
                    var list = _latestTransaction.GetList(id);
                    return Results.Ok(list);
                }
                catch
                {
                    return Results.Ok();
                }
            });
        }
    }
}