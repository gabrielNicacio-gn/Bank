using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SimplifiedBank.Application.DTOs;
using SimplifiedBank.Application.DTOs.Response;
using SimplifiedBank.Application.UseCases;
using SimplifiedBank.Domain.Repositories;
using SimplifiedBank.Interfaces.Exceptions;

namespace SimplifiedBank.Interfaces.Routes
{
    public static class Endpoints
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapGet("", () => new { hello = "World" });

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