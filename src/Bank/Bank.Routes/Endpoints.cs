
using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Application.Exceptions;
using Bank.Bank.Application.Extensions;
using Bank.Bank.Domain.Interfaces;
using Bank.Bank.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Bank.Routes
{
    public static class Endpoints
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapGet("/test",()=>"Hello World").RequireAuthorization();
            
            var userEndpoints = app.MapGroup("/user").WithTags("User");
            userEndpoints.MapPost("/register",
                async ([FromServices] IUserServices userServices, [FromBody] UserRegisterDto user) =>
                {
                    try
                    {
                        var register = await userServices.RegisterUserAndAccount(user);
                        return Results.Created<UserResponse>($"/user/{register.IdUser}", null);
                    }
                    catch (DuplicateUserNameException userNameDuplicateException)
                    {
                        return Results.Conflict(userNameDuplicateException.Message);
                    }
                }).Validate<UserRegisterDto>();
            userEndpoints.MapPost("/login", async ([FromServices]IUserServices userServices,[FromBody]UserLoginDto user) =>
            {
                try
                {
                    var login = await userServices.AccountLogin(user);
                    return Results.Ok();
                }
                catch (IncorrectEmailOrPasswordException incorrectEmailOrPasswordException)
                {
                    return Results.Unauthorized();
                }
            }).Validate<UserLoginDto>();
            userEndpoints.MapPost("/logout", async ([FromServices]IUserServices userServices) =>
            {
                var login = await userServices.Logout();
                return Results.Ok();
            });
            
            var accountEndpoints = app.MapGroup("/").WithTags("Account");
            accountEndpoints.MapGet("/accounts", async ([FromServices] IAccountServices accountService) =>
            {
                try
                {
                    var result = await accountService.GetAccount();
                    return Results.Ok(result);
                }
                catch (AccountNotExistException userNotExistException)
                {
                    return Results.NotFound(userNotExistException.Message);
                }
            }).RequireAuthorization();
            accountEndpoints.MapPost("/account", async ([FromServices]IAccountServices accountService,[FromBody]AddBalanceDto balance) 
                =>
            {
                try
                {
                    var addBalance = await accountService.AddBalance(balance);
                    return Results.Ok(addBalance);
                }
                catch (AccountNotExistException userNotExistException)
                {
                    return Results.NotFound(userNotExistException.Message);
                }
            }).RequireAuthorization();
            
            var transactionEndpoints = app.MapGroup("/").WithTags("Transaction");
            transactionEndpoints.MapPost("/transaction", async ([FromServices]ITransactionServices transactionServices,[FromBody] CreateNewTransactionDto dto) =>
            {
                try
                {
                    var transaction = await transactionServices.CreateTransaction(dto);
                    return Results.Created($"/transaction/{transaction.IdTransaction}", transaction);
                }
                catch (InsufficientBalanceException insufficientBalanceException)
                {
                    return Results.BadRequest(insufficientBalanceException.Message);
                }
                catch (AccountNotExistException userNotExistException)
                {
                    return Results.NotFound(userNotExistException.Message);
                }
                catch (TransactionBetweenAccountsFailsException transactionBetweenAccountsFailsException)
                {
                    return Results.BadRequest(transactionBetweenAccountsFailsException.Message);
                }
            }).RequireAuthorization();
        }
    }
}   