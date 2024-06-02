
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplifiedBank.Application.Authentication;
using SimplifiedBank.Application.DTOs.Login;
using SimplifiedBank.Application.DTOs.Request;
using SimplifiedBank.Application.DTOs.Response;
using SimplifiedBank.Domain.Interface;
using SimplifiedBank.Domain.Interfaces;
using SimplifiedBank.Interfaces.Exceptions;

namespace SimplifiedBank.Interfaces.Routes
{
    public static class AccountEndpoints
    {
        public static void MapAccountEndpoints(this WebApplication app)
        {
            var endpoints = app.MapGroup("/bank").WithTags("Bank");

            endpoints.MapPost("/login", [AllowAnonymous] ([FromServices] IConfiguration _config, [FromServices] TokenService _token,
            string email,
            string password) =>
            {
                LoginDTO login = new(email, password);
                var newToken = _token.GenerateToken(login, _config);
                if (newToken is null)
                    return Results.BadRequest("Falha ao gerar token");

                return Results.Ok(newToken);
            });

            endpoints.MapGet("/{id}/account", ([FromRoute] int id, [FromServices] IReturnAccount _returnAccount) =>
            {
                try
                {
                    var account = _returnAccount.GetAccount(id);
                    return Results.Ok(account);
                }
                catch (UserNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            }).RequireAuthorization();
        }
    }
}