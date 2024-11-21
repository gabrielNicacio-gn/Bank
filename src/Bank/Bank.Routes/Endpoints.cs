
using Bank.Bank.Application.DTOs.Request;
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
            userEndpoints.MapPost("/register",async ([FromServices]IUserServices userServices,[FromBody]UserRegisterDto user) =>
            {
                var register = await userServices.RegisterAccount(user);
                return register.IsSuccess
                    ? Results.Created($"/user/{register.Data!.IdUser}",null)
                    : Results.BadRequest();
            }).Validate<UserRegisterDto>();
            userEndpoints.MapPost("/login", async ([FromServices]IUserServices userServices,[FromBody]UserLoginDto user) =>
            {
                var login = await userServices.AccountLogin(user);
                return login.IsSuccess
                    ?Results.Ok()
                    :Results.Unauthorized();
            }).Validate<UserLoginDto>();
            userEndpoints.MapPost("/logout", async ([FromServices]IUserServices userServices) =>
            {
                var login = await userServices.Logout();
                return login.IsSuccess
                    ?Results.Ok()
                    :Results.BadRequest();
            });
            
            var accountEndpoints = app.MapGroup("/").WithTags("Account");
            accountEndpoints.MapGet("/accounts", async ([FromServices] IAccountServices accountService) =>
            {
                var result =await accountService.GetAccount();
                return Results.Ok(result);
            });
            accountEndpoints.MapPost("/account", async ([FromServices]IAccountServices accountService,[FromBody]AddBalanceDto balance) 
                =>
            {
                var addBalance = await accountService.AddBalance(balance);
                return addBalance.IsSuccess
                    ?Results.Ok(addBalance)
                    :Results.BadRequest();
            });
            
            var transactionEndpoints = app.MapGroup("/").WithTags("Transaction");
            transactionEndpoints.MapPost("/transaction", async ([FromServices]ITransactionServices transactionServices,[FromBody] CreateNewTransactionDto dto) =>
            {
                var transaction = await transactionServices.CreateTransaction(dto);
                return transaction.IsSuccess
                    ? Results.Created($"/transaction/{transaction.Data!.IdTransaction}", transaction)
                    : Results.BadRequest(transaction);
            }).RequireAuthorization();
        }
    }
}   