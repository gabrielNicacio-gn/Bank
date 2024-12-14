
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
            app.MapGet("/test",()=>"Hello World");
            
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
            accountEndpoints.MapGet("/accounts", async ([FromServices] IAccountServices accountService,[FromServices]ITokenService tokenService) =>
            {
                try
                {
                    var userId = tokenService.GetUserIdFromToken();
                    var result = await accountService.GetAccount(userId);
                    return Results.Ok(result);
                }
                catch (AccountNotExistException userNotExistException)
                {
                    return Results.NotFound(userNotExistException.Message);
                }
            }).RequireAuthorization();
            accountEndpoints.MapPost("/account", async ([FromServices]IAccountServices accountService, [FromServices]ITokenService tokenService,[FromBody]AddBalanceDto balance) 
                =>
            {
                try
                {
                    var userIdCurrent = tokenService.GetUserIdFromToken();
                    var addBalance = await accountService.AddBalance(balance, userIdCurrent);
                    return Results.Ok(addBalance);
                }
                catch (AccountNotExistException userNotExistException)
                {
                    return Results.NotFound(userNotExistException.Message);
                }
                catch (FailUpdateBalanceException failUpdateBalanceException)
                {
                    return Results.BadRequest(failUpdateBalanceException.Message);
                }
            }).RequireAuthorization();
            
            var transactionEndpoints = app.MapGroup("/").WithTags("Transaction");
            transactionEndpoints.MapPost("/transaction", async ([FromServices]ITransactionServices transactionServices,[FromServices]ITokenService tokenService,[FromBody] CreateNewTransactionDto dto) =>
            {
                try
                {
                    var userIdCurrent = tokenService.GetUserIdFromToken();
                    var transaction = await transactionServices.CreateNewTransaction(dto,userIdCurrent);
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
            transactionEndpoints.MapGet("/transactions", async ([FromServices]ITransactionServices transactionServices,[FromServices]ITokenService tokenService) =>
            {
                    var userIdCurrent = tokenService.GetUserIdFromToken();
                    var transactions = await transactionServices.GetLatestTransactions(userIdCurrent);
                    return Results.Ok(transactions);
            }).RequireAuthorization();
        }
    }
}   